using Microsoft.EntityFrameworkCore;

namespace Rodkulman.MilkMafia.Models
{
    public class MilkMafiaContext : DbContext
    {
        public DbSet<Category> Categories {get; set;}
        public DbSet<Product> Products {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity => 
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Product>(entity => 
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Category).WithMany(x => x.Products);
            });
        }
    }
}