using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
