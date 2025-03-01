using System.Linq.Expressions;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        // Retrieve all products with optional pagination, filtering, sorting, and related data
        Task<List<Product>> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            Expression<Func<Product, bool>>? filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
            bool includeRelated = false,
            CancellationToken cancellationToken = default);

        // Retrieve a specific product by ID with optional related data
        Task<Product?> GetByIdAsync(
            int id,
            bool includeRelated = false,
            CancellationToken cancellationToken = default);

        // Add a new product
        Task AddAsync(
            Product product,
            CancellationToken cancellationToken = default);

        // Update an existing product
        Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken = default);

        // Delete a product (soft delete if implemented)
        Task DeleteAsync(
            Product product,
            CancellationToken cancellationToken = default);

        // Retrieve only soft-deleted products (if soft delete is implemented)
        Task<List<Product>> GetDeletedAsync(CancellationToken cancellationToken = default);

        // Restore a soft-deleted product (if soft delete is implemented)
        Task RestoreAsync(
            Product product,
            CancellationToken cancellationToken = default);

        // Save changes explicitly (if using Unit of Work pattern)
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
