using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

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

        #region Extensions            
        public void UpdateValues(ProductViewModel model, MilkMafiaContext context)
        {
            this.CategoryId = model.CategoryId;
            this.MaterialId = model.MaterialId;
            this.Description = model.Description;
            this.ExpirationDays = model.ExpirationDays;
            this.STTax = model.STTax / 100;

            if (this.Prices == null)
            {
                this.Prices = new List<ProductPrice>();
            }
            else if (this.Prices.Any())
            {
                for (int i = this.Prices.Count - 1; i >= 0; i--)
                {
                    if (!model.Prices.Where(x => x != null).Any(x => x.Id == this.Prices.ElementAt(i).Id))
                    {
                        var priceToDelete = this.Prices.ElementAt(i);
                        this.Prices.Remove(priceToDelete);

                        context.Remove(priceToDelete);
                    }
                }
            }

            foreach (var priceModel in model.Prices.Where(x => x != null))
            {
                ProductPrice domain;

                if (priceModel.Id == 0)
                {
                    domain = new ProductPrice() { ProductId = this.Id, Product = this};
                    context.Add(domain);
                    this.Prices.Add(domain);
                }
                else
                {
                    domain = context.ProductPrices.First(x => x.Id == priceModel.Id);
                }

                domain.UpdateValues(priceModel, context);                
            }

            if (this.Paletization == null)
            {
                this.Paletization = new Paletization(){ ProductId = this.Id, Product = this};
                context.Paletization.Add(this.Paletization);
            }

            this.Paletization.UpdateValues(model);            
        }
        #endregion
    }
}
