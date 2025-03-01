using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProductManagement.Models
{
    public class Models
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;

            [Column(TypeName = "decimal(18, 2)")]
            public decimal Price { get; set; }

            public int StockQuantity { get; set; }
            public bool IsDeleted { get; set; }

            public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>(); // Many-to-Many
            public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // One-to-Many
        }

        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;

            public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>(); // Many-to-Many
        }

        public class Order
        {
            public int Id { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public DateTime OrderDate { get; set; }

            public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // One-to-Many
        }

        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public Order Order { get; set; } = new Order();

            public int ProductId { get; set; }
            public Product Product { get; set; } = new Product();

            public int Quantity { get; set; }

            [Column(TypeName = "decimal(18, 2)")]
            public decimal UnitPrice { get; set; }
        }

        public class ProductCategory
        {
            public int ProductId { get; set; }
            public Product Product { get; set; } = new Product();

            public int CategoryId { get; set; }
            public Category Category { get; set; } = new Category();
        }
    }
}