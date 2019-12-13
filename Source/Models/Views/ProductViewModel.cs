using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Rodkulman.MilkMafia.Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            this.Prices = new List<ProductPriceViewModel>();
        }
        
        public ProductViewModel(Product product)
        {
            if (product != null)
            {
                this.Id = product.Id;
                this.CategoryId = product.CategoryId;
                this.MaterialId = product.MaterialId;
                this.Description = product.Description;
                this.ExpirationDays = product.ExpirationDays;
                this.STTax = product.STTax * 100;
                this.Prices = product.Prices?.Select(x => new ProductPriceViewModel(x)).OrderByDescending(x => x.IsPrimary).ToList() ?? new List<ProductPriceViewModel>();
                this.BoxQuantity = product.Paletization?.BoxQuantity ?? 0;
                this.BoxLayerQuantity = product.Paletization?.BoxLayerQuantity ?? 0;
                this.LayerPalletQuantity = product.Paletization?.LayerPalletQuantity ?? 0;
            }
            else
            {
                this.Prices = new List<ProductPriceViewModel>();
            }
        }

        public int Id { get; set; }
        [Required]
        [Display(Name="Material")]
        public string MaterialId { get; set; }
        [Required]
        [Display(Name="Nome")]
        public string Description { get; set; }
        [Required]
        [Display(Name="Categoria")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name="Validade")]
        public int ExpirationDays { get; set; }
        [Required]
        [Display(Name="ST")]
        [DisplayFormat(DataFormatString="{0:N2}", ApplyFormatInEditMode = true)]
        public double STTax { get; set; }

        [Display(Name = "Preços")]
        public ICollection<ProductPriceViewModel> Prices { get; set; }

        #region Paletization
        [Display(Name = "Unidade por Caixa")]
        public int BoxQuantity { get; set; }
        [Display(Name = "Caixas por Lastro")]
        public int BoxLayerQuantity { get; set; }
        [Display(Name = "Lastros por Palete")]
        public int LayerPalletQuantity { get; set; }
        #endregion
        public ICollection<SelectListItem> AllCategories { get; set; }
    }
}
