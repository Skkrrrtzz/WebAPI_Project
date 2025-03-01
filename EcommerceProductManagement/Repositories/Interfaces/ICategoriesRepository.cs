using System.Linq.Expressions;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
            bool includeRelated = false,
            CancellationToken cancellationToken = default);

        Task<Category?> GetByIdAsync(
            int id,
            bool includeRelated = false,
            CancellationToken cancellationToken = default);

        Task<List<Category>> GetByIdsAsync(
    List<int> ids,
    bool includeRelated = false,
    CancellationToken cancellationToken = default);

        Task<Category?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Category category,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Category category,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            Category category,
            CancellationToken cancellationToken = default);
    }
}