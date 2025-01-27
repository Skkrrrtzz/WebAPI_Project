using EcommerceProductManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProductManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _service;

        public ReportsController(ReportsService service)
        {
            _service = service;
        }

        // 1. Get products by category
        [HttpGet("products-by-category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _service.GetProductsByCategoryAsync(categoryId);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found for category ID {categoryId}.");
            }
            return Ok(products);
        }

        // 2. Get orders from the last month
        [HttpGet("orders-last-month")]
        public async Task<IActionResult> GetOrdersFromLastMonth()
        {
            var orders = await _service.GetOrdersFromLastMonthAsync();
            return Ok(orders);
        }

        // 3. Get total sales per product
        [HttpGet("total-sales-per-product")]
        public async Task<IActionResult> GetTotalSalesPerProduct()
        {
            var totalSales = await _service.GetTotalSalesPerProductAsync();
            return Ok(totalSales);
        }

        // 4. Get top 5 products by sales
        [HttpGet("top-5-products")]
        public async Task<IActionResult> GetTop5ProductsBySales()
        {
            var topProducts = await _service.GetTop5ProductsBySalesAsync();
            return Ok(topProducts);
        }
    }
}