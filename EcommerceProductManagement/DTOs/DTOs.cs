namespace EcommerceProductManagement.DTOs
{
    public class DTOs
    {
        // For Product Management
        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public List<int> CategoryIds { get; set; } = new List<int>(); // List of Category IDs
        }

        public class CreateProductDto
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public List<int> CategoryIds { get; set; } = new List<int>(); // List of Category IDs
        }

        public class UpdateProductDto
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public decimal? Price { get; set; }
            public int? StockQuantity { get; set; }
            public List<int>? CategoryIds { get; set; } // List of Category IDs (optional for update)
        }

        // For Category Management
        public class CategoryDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
        }

        public class CreateCategoryDto
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
        }

        public class UpdateCategoryDto
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
        }

        // For Order Management
        public class OrderDto
        {
            public int Id { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public DateTime OrderDate { get; set; }
            public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        }

        public class CreateOrderDto
        {
            public string CustomerName { get; set; } = string.Empty;
            public DateTime OrderDate { get; set; }
            public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
        }

        public class UpdateOrderDto
        {
            public string? CustomerName { get; set; }
            public DateTime? OrderDate { get; set; }
        }

        public class OrderItemDto
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class CreateOrderItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        // For Reports Management
        public class ReportsProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
        }

        public class ReportsOrderDto
        {
            public int Id { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public DateTime OrderDate { get; set; }
            public List<ReportsOrderItemDto> OrderItems { get; set; } = new List<ReportsOrderItemDto>();
        }

        public class ReportsOrderItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal UnitPrice { get; set; }
        }

        public class ReportsProductSalesDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal TotalSales { get; set; }
        }
    }
}