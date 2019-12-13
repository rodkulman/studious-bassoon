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
        [Required]
        [Display(Name="Material")]
        public string MaterialId { get; set; }
        [Required]
        [Display(Name="Nome")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [Display(Name="Validade")]
        public int ExpirationDays { get; set; }
        [Required]
        [Display(Name="ST")]
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
            [JsonIgnore]
            public ProductPrice PrimaryPrice
            {
                get 
                {
                    return Prices?.FirstOrDefault(x => x.IsPrimary);
                }
            }
        #endregion
    }
}
