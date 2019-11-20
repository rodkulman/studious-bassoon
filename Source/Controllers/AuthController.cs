using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("api/v0/login")]
        public async Task<IActionResult> Login()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);

            var encoded = await reader.ReadToEndAsync();
            var bytes = Convert.FromBase64String(encoded);

            using var db = new MilkMafiaContext();

            if (db.Users.SingleOrDefault(x => x.CPF.SequenceEqual(bytes)) is User user)
            {
                var tokenList = await GetTokenList();

                var token = new JObject(
                    new JProperty("token", Guid.NewGuid()),
                    new JProperty("refresh", Guid.NewGuid()),
                    new JProperty("readAccess", user.ReadAccess),
                    new JProperty("writeAccess", user.WriteAccess),
                    new JProperty("expire", DateTime.Now.AddMinutes(30)),
                    new JProperty("expireRefresh", DateTime.Now.AddDays(1))
                );

                tokenList.Add(token);

                await SaveTokenList(tokenList);

                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("api/v0/refresh")]
        public async Task<IActionResult> Refresh()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);

            var refreshToken = await reader.ReadToEndAsync();

            var tokenList = await GetTokenList();

            if (tokenList.SingleOrDefault(x => x.Value<string>("refresh") == refreshToken) is JToken tokenData)
            {
                if (tokenData.Value<DateTime>("expireRefresh") > DateTime.Now)
                {
                    tokenData["token"] = Guid.NewGuid();
                    tokenData["refresh"] = Guid.NewGuid();
                    tokenData["expire"] = DateTime.Now.AddMinutes(30);
                    tokenData["expireRefresh"] = DateTime.Now.AddDays(1);

                    await SaveTokenList(tokenList);

                    return Ok(tokenData);
                }
            }

            return Unauthorized();
        }

        private static async Task SaveTokenList(JArray tokenList)
        {
            using (var file = System.IO.File.Open("DB/tokens.json", FileMode.Create, FileAccess.Write))
            using (var textWriter = new StreamWriter(file, Encoding.UTF8))
            using (var jWriter = new JsonTextWriter(textWriter))
            {
                await tokenList.WriteToAsync(jWriter);
            }
        }

        private static async Task<JArray> GetTokenList()
        {
            using (var file = System.IO.File.Open("DB/tokens.json", FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(file, Encoding.UTF8))
            using (var jReader = new JsonTextReader(textReader))
            {
                return await JArray.LoadAsync(jReader);
            }
        }
    }
}