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
    [Route("api/v0/Products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var categories = LoadCategories().ToArray();

            using (var reader = new StreamReader("products.csv"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');

                    if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[4])) { continue; }

                    yield return new Product()
                    {
                        Category = categories.First(x => x.Name == values[1]),
                        Id = values[2],
                        Name = values[3],
                        Price = GetPrice(values[4])
                    };
                }
            }            
        }

        [HttpGet("{categoryId}")]
        public IEnumerable<Product> Get(int categoryId)
        {
            var category = LoadCategories().First(x => x.Id == categoryId);

            using (var reader = new StreamReader("products.csv"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');

                    if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[4])) { continue; }

                    if (category.Name != values[1]) { continue; }

                    yield return new Product()
                    {
                        Category = category,
                        Id = values[2],
                        Name = values[3],
                        Price = GetPrice(values[4])
                    };
                }
            }
        }

        private IEnumerable<Category> LoadCategories()
        {
            int id = 0;
            using (var reader = new StreamReader("categories.csv"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return new Category() { Id = id++, Name = line };
                }
            }
        }

        private double GetPrice(string rawPrice)
        {
            return double.Parse(rawPrice.Trim().Substring(4), CultureInfo.GetCultureInfo("pt-br"));
        }
    }
}
