using EcommerceProductManagement.Repositories.Interfaces;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Services
{
    public class CategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoriesRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
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

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
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

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _categoriesRepository.UpdateAsync(category);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoriesRepository.DeleteAsync(category);
            return true;
        }
    }
}