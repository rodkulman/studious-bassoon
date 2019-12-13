using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rodkulman.MilkMafia.Models
{
    public class ProductPriceViewModel
    {
        public ProductPriceViewModel()
        {
            
        }
        
        public ProductPriceViewModel(ProductPrice price)
        {
            if (price != null)
            {
                this.Id = price.Id;
                this.Price = price.Price;
                this.Description = price.Description;
                this.IsPrimary = price.IsPrimary;
            }
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Preço")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Primário")]
        public bool IsPrimary { get; set; }
    }
}