using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;

namespace ARClothingAPI.BLL.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateUpdateDto categoryDto, string userId);
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<ApiResponse<CategoryDto>> GetByIdAsync(string id);
        Task<ApiResponse<CategoryDto>> UpdateAsync(string id, CategoryCreateUpdateDto categoryDto);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}
