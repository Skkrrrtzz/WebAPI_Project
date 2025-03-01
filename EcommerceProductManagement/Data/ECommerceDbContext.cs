using Microsoft.EntityFrameworkCore;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Electronics", Description = "All electronic devices, including gadgets, appliances, and accessories." },
        new Category { Id = 2, Name = "Fashion", Description = "Trendy clothing, shoes, and accessories for men, women, and kids." },
        new Category { Id = 3, Name = "Home & Kitchen", Description = "Furniture, appliances, cookware, and home decor items." },
        new Category { Id = 4, Name = "Beauty & Personal Care", Description = "Skincare, haircare, cosmetics, and grooming products." },
        new Category { Id = 5, Name = "Sports & Outdoors", Description = "Sports gear, fitness equipment, and outdoor adventure essentials." },
        new Category { Id = 6, Name = "Automotive", Description = "Car accessories, parts, and maintenance products." },
        new Category { Id = 7, Name = "Books & Stationery", Description = "Novels, textbooks, office supplies, and study materials." },
        new Category { Id = 8, Name = "Toys & Games", Description = "Kids' toys, board games, and video gaming accessories." },
        new Category { Id = 9, Name = "Health & Wellness", Description = "Supplements, fitness gear, and medical essentials." },
        new Category { Id = 10, Name = "Groceries & Essentials", Description = "Food, beverages, and daily household necessities." }
    );

            // Configure many-to-many relationship between Product and Category
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            // Configure one-to-many relationship between Order and OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Configure many-to-one relationship between OrderItem and Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // Configure decimal precision for Price and UnitPrice
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            // Add indexes for frequently queried fields
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderDate);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);

            // Add string length constraints
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(50);
        }
    }
}