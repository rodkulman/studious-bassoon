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
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            using (var reader = new StreamReader("categories.csv"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return new Category() { Name = line };
                }
            }
        }
    }
}
