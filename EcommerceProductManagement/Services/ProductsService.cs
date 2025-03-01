using EcommerceProductManagement.Repositories.Interfaces;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Services
{
    public class ProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository, ILogger<ProductsService> logger)
        {
            _productsRepository = productsRepository;
            _categoriesRepository = categoriesRepository;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(int page, int pageSize)
        {
            try
            {
                var products = await _productsRepository.GetAllAsync(
            page: page,
            pageSize: pageSize,
            includeRelated: true);

                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryIds = p.ProductCategories.Select(pc => pc.CategoryId).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");
                throw;
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productsRepository.GetByIdAsync(
           id: id,
           includeRelated: true);

                if (product == null) return null;

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving product with ID {id}.");
                throw;
            }
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            try
            {
                if (dto.CategoryIds == null || !dto.CategoryIds.Any())
                {
                    throw new InvalidOperationException("At least one category must be selected.");
                }

                var categories = await _categoriesRepository.GetByIdsAsync(dto.CategoryIds);

                if (categories.Count != dto.CategoryIds.Count)
                {
                    throw new InvalidOperationException("One or more categories do not exist.");
                }

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description ?? "N/A",
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity,
                    ProductCategories = categories.Select(c => new ProductCategory
                    {
                        CategoryId = c.Id
                    }).ToList()
                };

                await _productsRepository.AddAsync(product);
                await _productsRepository.SaveChangesAsync();

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                throw;
            }
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            try
            {
                var product = await _productsRepository.GetByIdAsync(id, includeRelated: true);
                if (product == null) return null;

                // Update product properties
                product.Name = dto.Name ?? product.Name;
                product.Description = dto.Description ?? product.Description;
                product.Price = dto.Price ?? product.Price;
                product.StockQuantity = dto.StockQuantity ?? product.StockQuantity;

                // Update categories if provided
                if (dto.CategoryIds != null)
                {
                    // Remove existing ProductCategory relationships
                    var existingProductCategories = product.ProductCategories.ToList();
                    foreach (var productCategory in existingProductCategories)
                    {
                        product.ProductCategories.Remove(productCategory);
                    }

                    // Add new ProductCategory relationships
                    foreach (var categoryId in dto.CategoryIds)
                    {
                        var category = await _categoriesRepository.GetByIdAsync(categoryId);
                        if (category == null)
                        {
                            throw new InvalidOperationException($"Category with ID {categoryId} does not exist.");
                        }

                        product.ProductCategories.Add(new ProductCategory
                        {
                            ProductId = product.Id,
                            CategoryId = category.Id
                        });
                    }
                }

                // Save changes
                await _productsRepository.UpdateAsync(product);

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating product with ID {id}.");
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productsRepository.GetByIdAsync(id);
                if (product == null) return false;

                await _productsRepository.DeleteAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting product with ID {id}.");
                throw;
            }
        }

        public class CategoryNotFoundException : Exception
        {
            public CategoryNotFoundException(int categoryId)
                : base($"Category with ID {categoryId} does not exist.")
            {
            }
        }
    }
}