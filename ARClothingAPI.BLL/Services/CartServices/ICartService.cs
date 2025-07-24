using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.CartServices
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetCartAsync(string userId);
        Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartRequest request);
        Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemRequest request);
        Task<ApiResponse<CartDto>> RemoveCartItemAsync(string userId, string productId);
        Task<ApiResponse<bool>> ClearCartAsync(string userId);
    }
}
