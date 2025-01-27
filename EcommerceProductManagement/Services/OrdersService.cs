using EcommerceProductManagement.Repositories.Interfaces;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Services
{
    public class OrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsRepository _productsRepository;

        public OrdersService(IOrdersRepository ordersRepository, IProductsRepository productsRepository)
        {
            _ordersRepository = ordersRepository;
            _productsRepository = productsRepository;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _ordersRepository.GetAllAsync();
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _ordersRepository.GetByIdAsync(id);
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                CustomerName = dto.CustomerName,
                OrderDate = dto.OrderDate,
                OrderItems = dto.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            // Update product stock based on order items
            foreach (var item in order.OrderItems)
            {
                var product = await _productsRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Insufficient stock for product {product.Name}.");

                product.StockQuantity -= item.Quantity;
                await _productsRepository.UpdateAsync(product);
            }

            await _ordersRepository.AddAsync(order);

            return new OrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }

        public async Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderDto dto)
        {
            var order = await _ordersRepository.GetByIdAsync(id);
            if (order == null) return null;

            order.CustomerName = dto.CustomerName;
            order.OrderDate = dto.OrderDate.Value;

            // Optional: Update order items logic if required
            // For simplicity, skipping updating order items here

            await _ordersRepository.UpdateAsync(order);

            return new OrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _ordersRepository.GetByIdAsync(id);
            if (order == null) return false;

            await _ordersRepository.DeleteAsync(order);
            return true;
        }
    }
}