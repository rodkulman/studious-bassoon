using System;
using System.Collections.Generic;

namespace Rodkulman.MilkMafia.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageId { get; set; } 

        public ICollection<Product> Products { get; set; }
    }
}