using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            using (var context = new MilkMafiaContext())
            {
                return context.Products.Include(x => x.Paletization).Include(x => x.Quantity).Include(x => x.Category).ToList();
            }
        }

        [HttpGet("{categoryId}")]
        public IEnumerable<Product> Get(int categoryId)
        {
            using (var context = new MilkMafiaContext())
            {
                return context.Products.Where(x => x.CategoryId == categoryId).Include(x => x.Paletization).Include(x => x.Quantity).Include(x => x.Category).ToList();
            }
        }
    }
}
