using System;
using System.Collections.Generic;

namespace Rodkulman.MilkMafia.Models
{
    public class Category
    {
        #region TableMapping
            public int Id { get; set; }
            public string Description { get; set; }
            public string ImageId { get; set; } 
        #endregion

        public ICollection<Product> Products { get; set; }
    }
}