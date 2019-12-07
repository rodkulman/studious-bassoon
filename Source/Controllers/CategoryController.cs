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
    [Route("api/v0/Categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            using (var context = new MilkMafiaContext())
            {
                return context.Categories.ToList();
            }
        }

        [HttpGet]
        [Route("All")]
        public IActionResult GetAll()
        {
            using var context = new MilkMafiaContext();

            var serializer = JsonSerializer.CreateDefault();
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var retval = context.Categories
                .Include(x => x.Products)
                    .ThenInclude(x => x.Prices)
                .Include(x => x.Products)
                    .ThenInclude(x => x.Images)
                .Include(x => x.Products)
                    .ThenInclude(x => x.Paletization)
                .ToList();

            var data = new JArray();
            
            serializer.Serialize(data.CreateWriter(), retval);

            return Ok(data);
        }
    }
}
