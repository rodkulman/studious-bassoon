using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia.Controllers
{
    [ApiController]
    [Route("api/v0/Import")]
    public class ImportController : ControllerBase
    {
        [HttpPost]
        [Route("csv")]
        public IActionResult UpdateProducts()
        {
            using var context = new MilkMafiaContext();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);

            var currentCategory = default(Category);
            var line = string.Empty;
            var brazilCulture = CultureInfo.GetCultureInfo("pt-br");

            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');

                if (string.IsNullOrWhiteSpace(values[1])) { continue; }

                if (values.Skip(2).All(string.IsNullOrWhiteSpace))
                {
                    // this is the start of another category block
                    currentCategory = context.Categories.FirstOrDefault(x => x.Description == values[1]);

                    if (currentCategory == null) 
                    {
                        currentCategory = new Category() { Description = values[1] }; 
                        context.Categories.Add(currentCategory);
                    }                    
                }
                else if (values.All(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // this a product
                    var product = context.Products.FirstOrDefault(x => x.MaterialId == values[0]);

                    if (product == null) 
                    {
                        product = new Product() { MaterialId = values[0], Category = currentCategory, CategoryId = currentCategory.Id }; 
                        currentCategory.Products.Add(product);
                    }

                    product.Description = values[3];
                    product.UnitPrice = double.Parse(values[4].Substring(4).Trim(), brazilCulture);
                    product.STTax = double.Parse(values[6][0..^2], brazilCulture) / 100.0;

                    if (string.IsNullOrWhiteSpace(values[7]))
                    {
                        product.STPrice = -1;
                    }
                    else
                    {
                        product.STPrice = double.Parse(values[7].Substring(4).Trim(), brazilCulture);
                    }

                    product.ExpirationDays = int.Parse(values[13]);                    

                    var match = Regex.Match(values[5].Trim(), @"^(?:acima de (?<qtty>\d+) cx )?R\$ (?<price>\d+,\d{1,2})$", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        if (product.Quantity == null)
                        {
                            if (product.Id == 0) 
                            {
                                product.Quantity = new ProductQuantity() { Product = product };
                            }
                            else
                            {
                                product.Quantity = new ProductQuantity() { Product = product, ProductId = product.Id };
                            }
                        }

                        if (match.Groups.Keys.Contains("qtty"))
                        {
                            product.Quantity.Quantity = int.Parse(match.Groups["qtty"].Value);
                        }
                        else
                        {
                            product.Quantity.Quantity = 10;
                        }

                        product.Quantity.Price = double.Parse(match.Groups["price"].Value, brazilCulture);
                    }

                    if (product.Paletization == null)
                    {
                        if (product.Id == 0) 
                        {
                            product.Paletization = new Paletization() { Product = product };
                        }
                        else
                        {
                            product.Paletization = new Paletization() { Product = product, ProductId = product.Id };
                        }
                    }

                    product.Paletization.BoxQuantity = int.Parse(values[10]);
                    product.Paletization.BoxLayerQuantity = int.Parse(values[12]);
                    product.Paletization.LayerPalletQuantity = int.Parse(values[11]) / product.Paletization.BoxLayerQuantity;
                }
            }

            context.SaveChanges();

            return Ok();
        }
    }
}