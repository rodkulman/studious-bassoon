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
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            using (var reader = new StreamReader("products.csv"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');

                    if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[4])) { continue; }

                    yield return new Product()
                    {
                        Category = new Category() { Name = values[1] },
                        Id = values[2],
                        Name = values[3],
                        Price = GetPrice(values[4])
                    };
                }
            }

            double GetPrice(string rawPrice)
            {
                return double.Parse(rawPrice.Trim().Substring(4), CultureInfo.GetCultureInfo("pt-br"));
            }
        }        
    }
}
