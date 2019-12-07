using System.Text.Json.Serialization;

namespace Rodkulman.MilkMafia.Models
{
    public class ProductImage
    {
        #region TableMapping
        public int Id { get; set; }
        public int ProductId { get; set; }
        
        [JsonIgnore]
        public string ImagePath { get; set; }
        #endregion

        #region Relations            
        public Product Product { get; set; }
        #endregion
    }
}