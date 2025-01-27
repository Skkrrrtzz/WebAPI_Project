using EcommerceProductManagement.Repositories.Interfaces;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Services
{
    public class ProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public ProductsService(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
        {
            _productsRepository = productsRepository;
            _categoriesRepository = categoriesRepository;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productsRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                // Include categories as a list of category IDs or category names in the DTO
                CategoryIds = p.ProductCategories.Select(pc => pc.CategoryId).ToList()
            }).ToList();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList() // Categories for the product
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            // Validate categories
            if (dto.CategoryIds == null || !dto.CategoryIds.Any())
            {
                throw new InvalidOperationException("At least one category must be selected.");
            }

            var categories = new List<Category>();
            foreach (var categoryId in dto.CategoryIds)
            {
                var category = await _categoriesRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    throw new InvalidOperationException($"Category with ID {categoryId} does not exist.");
                }
                categories.Add(category);
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description ?? "N/A",
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            await _productsRepository.AddAsync(product);

            // Create ProductCategory links (many-to-many relationship)
            foreach (var category in categories)
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = category.Id
                });
            }

            // Save product with categories
            await _productsRepository.UpdateAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList() // Return category IDs in DTO
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            if (product == null) return null;

            // Validate category IDs if provided
            if (dto.CategoryIds != null)
            {
                var categories = new List<Category>();
                foreach (var categoryId in dto.CategoryIds)
                {
                    var category = await _categoriesRepository.GetByIdAsync(categoryId);
                    if (category == null)
                    {
                        throw new InvalidOperationException($"Category with ID {categoryId} does not exist.");
                    }
                    categories.Add(category);
                }

                // Update the ProductCategories relationship (many-to-many)
                product.ProductCategories.Clear(); // Remove existing category links
                foreach (var category in categories)
                {
                    product.ProductCategories.Add(new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = category.Id
                    });
                }
            }

            product.Name = dto.Name ?? "N/A";
            product.Description = dto.Description ?? "N/A";
            product.Price = dto.Price ?? 0;
            product.StockQuantity = dto.StockQuantity ?? 0;

            await _productsRepository.UpdateAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList() // Return category IDs in DTO
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            if (product == null) return false;

            await _productsRepository.DeleteAsync(product);
            return true;
        }
    }

}