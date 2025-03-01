using EcommerceProductManagement.Repositories.Interfaces;
using System.Linq.Expressions;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Services
{
    public class CategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ILogger<CategoriesService> _logger;

        public CategoriesService(ICategoriesRepository categoriesRepository, ILogger<CategoriesService> logger)
        {
            _categoriesRepository = categoriesRepository;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync(
    int page = 1,
    int pageSize = 10,
    string? searchTerm = null)
        {
            // Define a filter if a search term is provided
            Expression<Func<Category, bool>>? filter = null;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filter = c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
            }

            // Retrieve categories with filtering, sorting, and pagination
            var categories = await _categoriesRepository.GetAllAsync(
                page: page,
                pageSize: pageSize,
                filter: filter,
                orderBy: query => query.OrderBy(c => c.Name),
                includeRelated: true);

            // Map to DTOs
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _categoriesRepository.GetByIdAsync(id);
                if (category == null) return null;

                return new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving category with ID {id}.");
                throw;
            }
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            try
            {
                // Check if a category with the same name already exists using the repository's GetByNameAsync
                var existingCategory = await _categoriesRepository.GetByNameAsync(dto.Name);

                if (existingCategory != null)
                {
                    // If category already exists, throw an exception or return a specific result
                    throw new InvalidOperationException("A category with the same name already exists.");
                }

                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description
                };

                await _categoriesRepository.AddAsync(category);

                return new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a category.");
                throw;
            }
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            try
            {
                var category = await _categoriesRepository.GetByIdAsync(id);
                if (category == null) return null;

                // Check if the updated name already exists for another category
                var existingCategory = await _categoriesRepository.GetByNameAsync(dto.Name);
                if (existingCategory != null && existingCategory.Id != id)
                {
                    throw new InvalidOperationException("A category with the same name already exists.");
                }

                category.Name = dto.Name ?? category.Name;
                category.Description = dto.Description ?? category.Description;

                await _categoriesRepository.UpdateAsync(category);

                return new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating category with ID {id}.");
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoriesRepository.GetByIdAsync(id);
                if (category == null) return false;

                await _categoriesRepository.DeleteAsync(category);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting category with ID {id}.");
                throw;
            }
        }
    }
}