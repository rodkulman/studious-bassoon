using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rodkulman.MilkMafia.Models;

namespace Rodkulman.MilkMafia.Controllers
{
    public class CategoryViewController : Controller
    {
        public IActionResult Index()
        {
            using var context = new MilkMafiaContext();

            return View(context.Categories.ToList());
        }
    }
}