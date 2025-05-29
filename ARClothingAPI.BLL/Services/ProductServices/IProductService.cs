using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Http;

namespace ARClothingAPI.BLL.Services.ProductServices
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDto>> CreateAsync(ProductCreateUpdateDto productDto, string userId);
        Task<ApiResponse<IEnumerable<ProductDto>>> GetAllAsync();
        Task<ApiResponse<ProductDto>> GetByIdAsync(string id);
        Task<ApiResponse<ProductDto>> UpdateAsync(string id, ProductCreateUpdateDto productDto);
        Task<ApiResponse<bool>> DeleteAsync(string id);

        // Phương thức xử lý upload ảnh
        Task<ApiResponse<ProductDto>> UploadImageAsync(string productId, IFormFile file);
        
        // Phương thức xử lý upload nhiều ảnh
        Task<ApiResponse<ProductDto>> UploadMultipleImagesAsync(string productId, List<IFormFile> files);
        
        // Phương thức xử lý xóa ảnh
        Task<ApiResponse<ProductDto>> DeleteImageAsync(string productId, string imageUrl);
    }
}
