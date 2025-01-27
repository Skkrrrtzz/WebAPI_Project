using EcommerceProductManagement.Data;
using Microsoft.EntityFrameworkCore;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories
{
    public class ReportsRepository
    {
        private readonly ECommerceDbContext _context;

        public ReportsRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        // 1. Retrieve all products in a specific category
        public async Task<IEnumerable<ReportsProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => new ReportsProductDto
                {
                    Id = pc.Product.Id,
                    Name = pc.Product.Name,
                    Description = pc.Product.Description,
                    Price = pc.Product.Price,
                    StockQuantity = pc.Product.StockQuantity
                })
                .ToListAsync();
        }

        // 2. Retrieve all orders placed within the last month
        public async Task<IEnumerable<ReportsOrderDto>> GetOrdersFromLastMonthAsync()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return await _context.Orders
                .Where(o => o.OrderDate >= lastMonth)
                .Select(o => new ReportsOrderDto
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems.Select(oi => new ReportsOrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        // 3. Retrieve the total sales for each product
        public async Task<IEnumerable<ReportsProductSalesDto>> GetTotalSalesPerProductAsync()
        {
            return await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(group => new ReportsProductSalesDto
                {
                    ProductId = group.Key,
                    ProductName = group.First().Product.Name,
                    TotalSales = group.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .ToListAsync();
        }

        // 4. Retrieve the top 5 products with the highest sales
        public async Task<IEnumerable<ReportsProductSalesDto>> GetTop5ProductsBySalesAsync()
        {
            return await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(group => new ReportsProductSalesDto
                {
                    ProductId = group.Key,
                    ProductName = group.First().Product.Name,
                    TotalSales = group.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(result => result.TotalSales)
                .Take(5)
                .ToListAsync();
        }
    }
}