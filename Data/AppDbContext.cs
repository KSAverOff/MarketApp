using MarketApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<SupplyItem> SupplyItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).HasColumnName("Name");
            entity.Property(p => p.Quantity).HasColumnName("Quantity");
            entity.Property(p => p.Units).HasConversion<string>();
            entity.Property(p => p.CategoryId).HasColumnName("CategoryId");

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).HasColumnName("Name");
        });

        // Sale + SaleItem
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sales");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Date).HasColumnName("Date");

            entity.HasMany(s => s.Items)
                  .WithOne(si => si.Sale)
                  .HasForeignKey(si => si.SaleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("sale_items");
            entity.HasKey(si => si.Id);
            entity.Property(si => si.Quantity).HasColumnName("Quantity");

            entity.HasOne(si => si.Product)
                  .WithMany()
                  .HasForeignKey(si => si.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Supply + SupplyItem
        modelBuilder.Entity<Supply>(entity =>
        {
            entity.ToTable("supplies");
            entity.HasKey(sp => sp.Id);
            entity.Property(sp => sp.Date).HasColumnName("Date");

            entity.HasMany(sp => sp.Items)
                  .WithOne(spi => spi.Supply)
                  .HasForeignKey(spi => spi.SupplyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SupplyItem>(entity =>
        {
            entity.ToTable("supply_items");
            entity.HasKey(spi => spi.Id);
            entity.Property(spi => spi.Quantity).HasColumnName("Quantity");

            entity.HasOne(spi => spi.Product)
                  .WithMany()
                  .HasForeignKey(spi => spi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
