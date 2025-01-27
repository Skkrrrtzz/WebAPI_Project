using EcommerceProductManagement.Data;
using EcommerceProductManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ECommerceDbContext _context;

        public CategoriesRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                                 .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}