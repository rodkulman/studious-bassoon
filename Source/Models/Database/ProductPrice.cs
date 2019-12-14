using System;
using System.Text.Json.Serialization;

namespace Rodkulman.MilkMafia.Models
{
    public class ProductPrice
    {
        #region TableMapping
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
        #endregion

        #region Relations            
        public Product Product { get; set; }        
        #endregion

        #region Extensions
        public void UpdateValues(ProductPriceViewModel priceModel, MilkMafiaContext context)
        {
            this.Price = priceModel.Price;
            this.Description = priceModel.Description;
            this.IsPrimary = priceModel.IsPrimary;
        }
        #endregion
    }
}