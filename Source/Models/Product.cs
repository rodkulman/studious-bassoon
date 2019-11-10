using System;

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
        public double UnitPrice { get; set; }
        public double STTax { get; set; }
        public double STPrice { get; set; }
        public string ImageId { get; set; } 

        #endregion

        #region Relations
            public Category Category { get; set; }
            public ProductQuantity Quantity { get; set; }
            public Paletization Paletization { get; set; }
        #endregion
    }
}
