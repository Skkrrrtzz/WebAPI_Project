using EcommerceProductManagement.Data;
using EcommerceProductManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public async Task<List<Category>> GetAllAsync(
            int page = 1,
            int pageSize = 10,
            Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
            bool includeRelated = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Category> query = _context.Categories;

                // Apply filtering if provided
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Include related data if requested
                if (includeRelated)
                {
                    query = query
                        .Include(c => c.ProductCategories)
                        .ThenInclude(pc => pc.Product);
                }

                // Apply sorting if provided
                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                // Apply pagination
                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                // Execute the query
                return await query
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task<Category?> GetByIdAsync(
            int id,
            bool includeRelated = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Category> query = _context.Categories;

                // Include related data if requested
                if (includeRelated)
                {
                    query = query
                        .Include(c => c.ProductCategories)
                        .ThenInclude(pc => pc.Product);
                }

                // Execute the query
                return await query
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving category with ID {id}.", ex);
            }
        }

        public async Task<List<Category>> GetByIdsAsync(
    List<int> ids,
    bool includeRelated = false,
    CancellationToken cancellationToken = default)
        {
            try
            {
                if (ids == null || !ids.Any())
                {
                    return new List<Category>(); // Return empty list if no IDs provided
                }

                IQueryable<Category> query = _context.Categories.Where(c => ids.Contains(c.Id));

                // Include related data if requested
                if (includeRelated)
                {
                    query = query
                        .Include(c => c.ProductCategories)
                        .ThenInclude(pc => pc.Product);
                }

                return await query.AsNoTracking().ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving categories with IDs: {string.Join(", ", ids)}", ex);
            }
        }

        public async Task<Category?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving category with name {name}.", ex);
            }
        }

        public async Task AddAsync(
            Category category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while adding a category.", ex);
            }
        }

        public async Task UpdateAsync(
            Category category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while updating a category.", ex);
            }
        }

        public async Task DeleteAsync(
            Category category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while deleting a category.", ex);
            }
        }
    }
}