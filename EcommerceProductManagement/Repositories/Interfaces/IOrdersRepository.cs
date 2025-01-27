using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task AddAsync(Order order);

        Task UpdateAsync(Order order);

        Task DeleteAsync(Order order);
    }
}