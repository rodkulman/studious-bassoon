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
    [Route("api/v0/Images")]
    public class ImageController : ControllerBase
    {
        [HttpGet]
        [Route("product/{imageId}")]
        public IActionResult GetProductImage(int imageId)
        {
            using var context = new MilkMafiaContext();

            var image = context.ProductImages.SingleOrDefault(x => x.Id == imageId);

            if (image != null && System.IO.File.Exists($"images/{image.ImagePath}"))
            {
                var retVal = System.IO.File.ReadAllBytes($"images/{image.ImagePath}");

                return File(retVal, "image/png");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("category/{categoryId}")]
        public IActionResult GetCategoryImage(int categoryId, [FromQuery]string size)
        {
            if (size != "large" && size != "small")
            {
                return BadRequest($"{nameof(size)} must be either 'large' or 'small'");
            }

            using var context = new MilkMafiaContext();

            var category = context.Categories.SingleOrDefault(x => x.Id == categoryId);

            if (category != null)
            {
                string imagePath;

                if (size == "large") { imagePath = category.LargeImagePath; }
                else { imagePath = category.SmallImagePath; }

                if (System.IO.File.Exists("images/{imagePath}"))
                {
                    var retVal = System.IO.File.ReadAllBytes($"images/{imagePath}");

                    return File(retVal, "image/png");
                }
                else
                {
                    return StatusCode(500, "File does not exist");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
