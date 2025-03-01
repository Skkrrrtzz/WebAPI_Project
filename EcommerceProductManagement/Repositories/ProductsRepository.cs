using static EcommerceProductManagement.Models.Models;
using System;
using EcommerceProductManagement.Repositories.Interfaces;
using EcommerceProductManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcommerceProductManagement.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductsRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync(int page = 1,
            int pageSize = 10,
            Expression<Func<Product, bool>>? filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
            bool includeRelated = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Product> query = _context.Products;

                // Apply filtering if provided
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Include related data if requested
                if (includeRelated)
                {
                    query = query
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category);
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
                throw new RepositoryException("An error occurred while retrieving products.", ex);
            }
        }

        public async Task<Product?> GetByIdAsync(int id,
            bool includeRelated = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Product> query = _context.Products;

                // Include related data if requested
                if (includeRelated)
                {
                    query = query
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category);
                }

                // Execute the query
                return await query
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving product with ID {id}.", ex);
            }
        }

        public async Task AddAsync(Product product,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while adding a product.", ex);
            }
        }

        public async Task UpdateAsync(Product product,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while updating a product.", ex);
            }
        }

        public async Task DeleteAsync(Product product,
            CancellationToken cancellationToken = default)
        {
            try
            {
                product.IsDeleted = true;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while deleting a product.", ex);
            }
        }

        public async Task<List<Product>> GetDeletedAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while retrieving deleted products.", ex);
            }
        }

        public async Task RestoreAsync(
            Product product,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Restore soft-deleted product
                product.IsDeleted = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while restoring a product.", ex);
            }
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred while saving changes.", ex);
            }
        }
    }

    public class RepositoryException : Exception
    {
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}