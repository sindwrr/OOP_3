using ShopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.DB
{
    // контекст для работы с БД
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // указание полей в таблицах и связей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Address).IsRequired();
            });

            modelBuilder.Entity<ProductStock>(entity =>
            {
                entity.HasKey(e => new {e.ShopCode, e.ProductName});
                entity.Property(e => e.ShopCode).IsRequired();
                entity.Property(e => e.ProductName).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).IsRequired();

                // продукты связаны с магазином по коду магазина
                entity.HasOne<Shop>()
                      .WithMany()
                      .HasForeignKey(e => e.ShopCode)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
