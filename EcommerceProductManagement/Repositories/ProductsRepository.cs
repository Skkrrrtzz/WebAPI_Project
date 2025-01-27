using static EcommerceProductManagement.Models.Models;
using System;
using EcommerceProductManagement.Repositories.Interfaces;
using EcommerceProductManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProductManagement.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductsRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

}
