using ARClothingAPI.Common.DTOs;

namespace ARClothingAPI.BLL.Services.CartServices
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(string userId);
        Task<CartDto> AddToCartAsync(string userId, AddToCartRequest request);
        Task<CartDto?> UpdateCartItemAsync(string userId, UpdateCartItemRequest request);
        Task<bool> RemoveCartItemAsync(string userId, string productId);
        Task<bool> ClearCartAsync(string userId);
    }
}
