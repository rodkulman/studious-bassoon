using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rodkulman.MilkMafia.Models
{
    public class Category
    {
        #region TableMapping
            public int Id { get; set; }
            public string Description { get; set; }
            [JsonIgnore]
            public string LargeImagePath { get; set; } 
            [JsonIgnore]
            public string SmallImagePath { get; set; } 
        #endregion
        
        public ICollection<Product> Products { get; set; }
    }
}