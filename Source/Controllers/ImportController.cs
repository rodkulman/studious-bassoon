using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia.Controllers
{
    [ApiController]
    [Route("api/v0/Import")]
    public class ImportController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UpdateProducts()
        {
            using var context = new MilkMafiaContext();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);

            var currentCategory = default(Category);
            var line = string.Empty;
            var brazilCulture = CultureInfo.GetCultureInfo("pt-br");

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var values = line.Split(';').SkipLast(1).ToArray();

                if (string.IsNullOrWhiteSpace(values[1])) { continue; }

                if (values.Skip(2).All(string.IsNullOrWhiteSpace))
                {
                    // this is the start of another category block
                    currentCategory = context.Categories.FirstOrDefault(x => x.Description == values[1]);

                    if (currentCategory == null)
                    {
                        currentCategory = new Category() { Description = values[1], Products = new List<Product>() };
                        context.Categories.Add(currentCategory);
                    }
                }
                else if (values[0] == values[2] && !string.IsNullOrEmpty(values[0]))
                {
                    // this a product
                    var product = context.Products.FirstOrDefault(x => x.MaterialId == values[0]);

                    if (product == null)
                    {
                        product = new Product() { MaterialId = values[0], Category = currentCategory };
                        currentCategory.Products.Add(product);
                        context.Products.Add(product);
                    }

                    product.Description = values[3];

                    if (string.IsNullOrWhiteSpace(values[4]))
                    {
                        product.UnitPrice = -1;
                    }
                    else
                    {
                        product.UnitPrice = double.Parse(values[4].Substring(4).Trim(), brazilCulture);
                    }

                    if (!string.IsNullOrWhiteSpace(values[6]) && values[6].Contains("%"))
                    {
                        product.STTax = double.Parse(values[6][0..^2], brazilCulture) / 100.0;
                    }
                    else
                    {
                        product.STTax = -1;
                    }


                    if (int.TryParse(values[13], out int expirationDays))
                    {
                        product.ExpirationDays = expirationDays;
                    }
                    else
                    {
                        product.ExpirationDays = -1;
                    }

                    var match = Regex.Match(values[5].Trim(), @"^(?:acima de (?<qtty>\d+) cx )?R\$ (?<price>\d+,\d{1,2})$", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        if (!(product.Quantity?.Any() ?? false))
                        {
                            if (product.Quantity == null) { product.Quantity = new List<ProductQuantity>(); }

                            product.Quantity.Add(new ProductQuantity() { Product = product });
                            context.ProductQuantity.Add(product.Quantity.Last());
                        }

                        if (match.Groups.Keys.Contains("qtty") && !string.IsNullOrEmpty(match.Groups["qtty"].Value))
                        {
                            product.Quantity.Last().Quantity = int.Parse(match.Groups["qtty"].Value);
                        }
                        else
                        {
                            product.Quantity.Last().Quantity = 10;
                        }

                        product.Quantity.Last().Price = double.Parse(match.Groups["price"].Value, brazilCulture);
                    }

                    if (product.Paletization == null)
                    {
                        product.Paletization = new Paletization() { Product = product };
                        context.Paletization.Add(product.Paletization);
                    }

                    if (int.TryParse(values[10], out int boxQuantity))
                    {
                        product.Paletization.BoxQuantity = boxQuantity;
                    }
                    else
                    {
                        product.Paletization.BoxQuantity = -1;
                    }

                    if (int.TryParse(values[12], out int boxLayerQuantity))
                    {
                        product.Paletization.BoxLayerQuantity = boxLayerQuantity;
                    }
                    else
                    {
                        product.Paletization.BoxLayerQuantity = -1;
                    }

                    if (product.Paletization.BoxLayerQuantity > 0 && int.TryParse(values[11], out int layerPalletQuantity))
                    {
                        product.Paletization.LayerPalletQuantity = layerPalletQuantity / product.Paletization.BoxLayerQuantity;
                    }
                    else
                    {
                        product.Paletization.LayerPalletQuantity = -1;
                    }
                }
            }

            context.SaveChanges();

            return Ok();
        }
    }
}