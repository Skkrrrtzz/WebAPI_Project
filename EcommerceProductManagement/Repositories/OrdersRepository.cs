using static EcommerceProductManagement.Models.Models;
using System;
using EcommerceProductManagement.Repositories.Interfaces;
using EcommerceProductManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProductManagement.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ECommerceDbContext _context;

        public OrdersRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}