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
    [Route("api/v0/Categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            int id = 0;
            using (var reader = new StreamReader("categories.csv"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return new Category() { Id = id++, Name = line, ImageId = "placeholder" };
                }
            }
        }
    }
}
