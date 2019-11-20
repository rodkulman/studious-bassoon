using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.EndsWith("api/v0/login"))
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);

                var encoded = await reader.ReadToEndAsync();
                var bytes = Convert.FromBase64String(encoded);

                using var db = new MilkMafiaContext();

                if (db.Users.SingleOrDefault(x => x.CPF.SequenceEqual(bytes)) is User user)
                {
                    JArray tokenList;

                    using (var file = File.Open("DB/tokens.json", FileMode.Open, FileAccess.Read))
                    using (var textReader = new StreamReader(file, Encoding.UTF8))
                    using (var jReader = new JsonTextReader(textReader))
                    {
                        tokenList = await JArray.LoadAsync(jReader);
                    }

                    var token = new JObject(
                        new JProperty("token", Guid.NewGuid().ToString()),
                        new JProperty("readAccess", user.ReadAccess),
                        new JProperty("writeAccess", user.WriteAccess),
                        new JProperty("expire", DateTime.Now.AddMinutes(30))
                    );

                    tokenList.Add(token);

                    using (var file = File.Open("DB/tokens.json", FileMode.Create, FileAccess.Write))
                    using (var textWriter = new StreamWriter(file, Encoding.UTF8))
                    using (var jWriter = new JsonTextWriter(textWriter))
                    {
                        await tokenList.WriteToAsync(jWriter);
                    }

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(token.ToString(), Encoding.UTF8);

                    return;
                }
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            else if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader) && authHeader[0].StartsWith("Bearer"))
            {
                JArray tokenList;

                using (var file = File.Open("DB/tokens.json", FileMode.OpenOrCreate, FileAccess.Read))
                using (var textReader = new StreamReader(file, Encoding.UTF8))
                using (var jReader = new JsonTextReader(textReader))
                {
                    tokenList = await JArray.LoadAsync(jReader);
                }

                var token = authHeader[0].Substring(7);

                if (tokenList.SingleOrDefault(x => x.Value<string>("token") == token) is JToken tokenData)
                {
                    if (tokenData.Value<DateTime>("expire") <= DateTime.Now)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new JObject(
                            new JProperty("Expire", tokenData.Value<DateTime>("expire")),
                            new JProperty("Reason", "expired"),
                            new JProperty("Message", "Token expired, please generate a new one.")
                        );

                        await context.Response.WriteAsync(response.ToString(), Encoding.UTF8);

                        return;
                    }
                    else if (!tokenData.Value<bool>("writeAccess") && context.Request.Method != "GET")
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                    else if (!tokenData.Value<bool>("readAccess"))
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
                return;
            }

            await next(context);
        }
    }
}