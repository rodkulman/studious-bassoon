using System;

namespace Rodkulman.MilkMafia.Models
{
    public class Product
    {
        public Category Category { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageId { get; set; }
    }
}
