using System;
using System.Collections.Generic;

namespace Rodkulman.MilkMafia.Models
{
    public class Product
    {
        #region TableMapping
        public int Id { get; set; }
        public string MaterialId { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ExpirationDays { get; set; }
        public double STTax { get; set; }
        public int? ImageId { get; set; }

        #endregion

        #region Relations
        public Category Category { get; set; }
        public ICollection<ProductPrice> Prices { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public Paletization Paletization { get; set; }
        #endregion
    }
}
