using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Rodkulman.MilkMafia.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel() { }
        public CategoryViewModel(Category model)
        {
            this.Id = model.Id;
            this.Description = model.Description;

            this.Products = model.Products.OrderBy(x => x.Description).Select(x => new ProductViewModel(x)).ToList();
        }

        #region TableMapping
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Description { get; set; }
        #endregion

        public ICollection<ProductViewModel> Products { get; set; }
    }
}