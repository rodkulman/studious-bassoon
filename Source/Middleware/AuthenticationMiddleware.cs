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
            if (!context.Request.Path.Value.StartsWith("/api") || context.Request.Path.Value == "/api/v0/login" || context.Request.Path.Value == "/api/v0/refresh")
            {
                await next(context);
            }
            else if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader) && authHeader[0].StartsWith("Bearer"))
            {
                var tokenList = await GetTokenList();

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
                    }
                    else if (!tokenData.Value<bool>("readAccess") || (!tokenData.Value<bool>("writeAccess") && context.Request.Method != "GET"))
                    {
                        context.Response.StatusCode = 401;
                    }
                    else
                    {
                        await next(context);
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
            }            
        }

        private static async Task<JArray> GetTokenList()
        {
            using (var file = File.Open("DB/tokens.json", FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(file, Encoding.UTF8))
            using (var jReader = new JsonTextReader(textReader))
            {
                return await JArray.LoadAsync(jReader);
            }
        }
    }
}