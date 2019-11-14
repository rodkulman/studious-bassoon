using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rodkulman.MilkMafia.Models
{
    public class MilkMafiaContext : DbContext
    {
        public DbSet<Category> Categories {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<ProductQuantity> ProductQuantity {get; set;}
        public DbSet<Paletization> Paletization {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            using(var file = File.Open("DB/config.json", FileMode.Open, FileAccess.Read))
            using(var reader = new StreamReader(file, Encoding.UTF8))
            using(var jReader = new JsonTextReader(reader))
            {
                var config = JObject.Load(jReader);

                optionsBuilder.UseMySQL($"server={config["server"].Value<string>()};database={config["dbname"].Value<string>()};user={config["user"].Value<string>()};password={config["password"].Value<string>()}");
            }            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity => 
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Paletization>(entity => 
            {
                entity.HasKey(x => x.ProductId);
            });

            modelBuilder.Entity<ProductQuantity>(entity => 
            {
                entity.HasKey(x => new { x.ProductId, x.Quantity });
            });

            modelBuilder.Entity<Product>(entity => 
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Category).WithMany(x => x.Products);
                entity.HasOne(x => x.Quantity).WithOne(x => x.Product);
                entity.HasOne(x => x.Paletization).WithOne(x => x.Product);
            });
        }
    }
}