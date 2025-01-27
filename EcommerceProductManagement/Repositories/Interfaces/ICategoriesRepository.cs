using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<Category?> GetByNameAsync(string name);

        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(Category category);
    }
}