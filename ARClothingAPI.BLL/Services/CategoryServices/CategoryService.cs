using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;
using MongoDB.Driver;

namespace ARClothingAPI.BLL.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateUpdateDto categoryDto, string userId)
        {
            if (categoryDto.ParentCategoryId != null)
            {
                var parentCategory = await _uow.Categories.GetByIdAsync(categoryDto.ParentCategoryId);
                if (parentCategory == null)
                    return ApiResponse<CategoryDto>.ErrorResult("Parent category not found");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl = categoryDto.ImageUrl,
                Order = categoryDto.Order,
                ParentCategoryId = categoryDto.ParentCategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _uow.Categories.InsertAsync(category);
            await _uow.CommitAsync();

            var categoryResponse = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                Order = category.Order,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return ApiResponse<CategoryDto>.SuccessResult(categoryResponse, "Category created successfully");
        }

        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Order = c.Order,
                ParentCategoryId = c.ParentCategoryId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).OrderBy(x => x.Order);

            return ApiResponse<IEnumerable<CategoryDto>>.SuccessResult(categoryDtos);
        }

        public async Task<ApiResponse<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
                return ApiResponse<CategoryDto>.ErrorResult("Category not found");

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                Order = category.Order,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return ApiResponse<CategoryDto>.SuccessResult(categoryDto);
        }

        public async Task<ApiResponse<CategoryDto>> UpdateAsync(string id, CategoryCreateUpdateDto categoryDto)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
                return ApiResponse<CategoryDto>.ErrorResult("Category not found");

            if (categoryDto.ParentCategoryId != null && categoryDto.ParentCategoryId != category.ParentCategoryId)
            {
                var parentCategory = await _uow.Categories.GetByIdAsync(categoryDto.ParentCategoryId);
                if (parentCategory == null)
                    return ApiResponse<CategoryDto>.ErrorResult("Parent category not found");
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.ImageUrl = categoryDto.ImageUrl;
            category.Order = categoryDto.Order;
            category.ParentCategoryId = categoryDto.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;

            await _uow.Categories.UpdateAsync(id, category);
            await _uow.CommitAsync();

            var updatedCategoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                Order = category.Order,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return ApiResponse<CategoryDto>.SuccessResult(updatedCategoryDto, "Category updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
                return ApiResponse<bool>.ErrorResult("Category not found");

            // Check if there are any products using this category
            var productsFilter = Builders<Product>.Filter.Eq(p => p.CategoryId, id);
            var products = await _uow.Products.FindAsync(productsFilter);
            if (products.Any())
                return ApiResponse<bool>.ErrorResult("Cannot delete category with associated products");

            // Check if there are any child categories
            var childCategoriesFilter = Builders<Category>.Filter.Eq(c => c.ParentCategoryId, id);
            var childCategories = await _uow.Categories.FindAsync(childCategoriesFilter);
            if (childCategories.Any())
                return ApiResponse<bool>.ErrorResult("Cannot delete category with child categories");

            await _uow.Categories.DeleteAsync(id);
            await _uow.CommitAsync();

            return ApiResponse<bool>.SuccessResult(true, "Category deleted successfully");
        }
    }
}
