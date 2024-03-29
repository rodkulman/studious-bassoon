using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rodkulman.MilkMafia.Models
{
    public class MilkMafiaContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Paletization> Paletization { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            using (var file = File.Open("DB/config.json", FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(file, Encoding.UTF8))
            using (var jReader = new JsonTextReader(reader))
            {
                var config = JObject.Load(jReader);

                optionsBuilder.UseMySQL($"server={config["server"].Value<string>()};database={config["dbname"].Value<string>()};user={config["user"].Value<string>()};password={config["password"].Value<string>()}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity => 
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ReadAccess).HasColumnType("bit");
                entity.Property(x => x.WriteAccess).HasColumnType("bit");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Paletization>(entity =>
            {
                entity.HasKey(x => x.ProductId);
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.IsPrimary).HasColumnType("bit");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Category).WithMany(x => x.Products);
                entity.HasMany(x => x.Prices).WithOne(x => x.Product);
                entity.HasMany(x => x.Images).WithOne(x => x.Product);
                entity.HasOne(x => x.Paletization).WithOne(x => x.Product);
            });
        }
    }
}