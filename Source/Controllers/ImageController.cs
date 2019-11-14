using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia.Controllers
{
    [ApiController]
    [Route("api/v0/Images")]
    public class ImageController : ControllerBase
    {
        [HttpGet]
        [Route("{imageId}")]
        public IActionResult Get(string imageId)
        {
            if (System.IO.File.Exists($"images/{imageId}.png"))
            {
                var retVal = System.IO.File.ReadAllBytes($"images/{imageId}.png");

                return File(retVal, "image/png");
            }
            else
            {
                var retVal = System.IO.File.ReadAllBytes($"images/placeholder.png");

                return File(retVal, "image/png");
            }
        }
    }
}
