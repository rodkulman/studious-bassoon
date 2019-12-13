using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia.Controllers
{
    public class ProductViewController : Controller
    {
        public IActionResult Index()
        {
            using var context = new MilkMafiaContext();

            var categories = context.Categories
                .Include(x => x.Products)
                    .ThenInclude(x => x.Paletization)
                .Include(x => x.Products)
                    .ThenInclude(x => x.Prices)
                .OrderBy(x => x.Description)
                .ToList();

            foreach (var category in categories)
            {
                category.Products = category.Products.OrderBy(x => x.Description).ToList();
            }

            return View(categories);
        }

        [HttpGet]
        public IActionResult Product(int id)
        {
            using var context = new MilkMafiaContext();

            var product = context.Products.Include(x => x.Paletization).Include(x => x.Prices).FirstOrDefault(x => x.Id == id);

            var view = new ProductViewModel(product);
            view.AllCategories = context.Categories.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Description}).ToList();

            return PartialView(view);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new MilkMafiaContext())
                {
                    context.SaveChanges();
                }
            }

            return PartialView(model);
        }
    }
}