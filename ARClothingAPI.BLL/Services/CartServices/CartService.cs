using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;

        public CartService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // Lấy giỏ hàng của user
        public async Task<ApiResponse<CartDto>> GetCartAsync(string userId)
        {
            var cart = await _uow.Carts.GetByUserIdAsync(userId);
            if (cart == null)
                return ApiResponse<CartDto>.ErrorResult("Cart not found");

            var cartDto = MapToCartDto(cart);
            return ApiResponse<CartDto>.SuccessResult(cartDto);
        }

        // Thêm sản phẩm vào giỏ hàng
        public async Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var cart = await _uow.Carts.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                // Lấy thông tin sản phẩm từ DB
                var product = await _uow.Products.GetByIdAsync(request.ProductId);
                if (product == null)
                    return ApiResponse<CartDto>.ErrorResult("Product not found");

                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = request.Quantity,
                    Price = product.Price,
                    ImageUrl = product.Images?.FirstOrDefault() ?? "",
                    Model3DUrl = product.Model3DUrl
                });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            if (cart.Id == default)
                await _uow.Carts.InsertAsync(cart);
            else
                await _uow.Carts.UpdateAsync(cart.Id.ToString(), cart);

            await _uow.CommitAsync();
            return ApiResponse<CartDto>.SuccessResult(MapToCartDto(cart), "Added to cart");
        }

        // Cập nhật số lượng sản phẩm trong giỏ
        public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemRequest request)
        {
            var cart = await _uow.Carts.GetByUserIdAsync(userId);
            if (cart == null)
                return ApiResponse<CartDto>.ErrorResult("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                return ApiResponse<CartDto>.ErrorResult("Cart item not found");

            item.Quantity = request.Quantity;
            cart.UpdatedAt = DateTime.UtcNow;

            await _uow.Carts.UpdateAsync(cart.Id.ToString(), cart);
            await _uow.CommitAsync();
            return ApiResponse<CartDto>.SuccessResult(MapToCartDto(cart), "Cart updated");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<ApiResponse<CartDto>> RemoveCartItemAsync(string userId, string productId)
        {
            var cart = await _uow.Carts.GetByUserIdAsync(userId);
            if (cart == null)
                return ApiResponse<CartDto>.ErrorResult("Cart not found");

            cart.Items.RemoveAll(i => i.ProductId == productId);
            cart.UpdatedAt = DateTime.UtcNow;

            await _uow.Carts.UpdateAsync(cart.Id.ToString(), cart);
            await _uow.CommitAsync();

            return ApiResponse<CartDto>.SuccessResult(MapToCartDto(cart), "Item removed from cart");
        }

        // Xóa toàn bộ giỏ hàng
        public async Task<ApiResponse<bool>> ClearCartAsync(string userId)
        {
            var cart = await _uow.Carts.GetByUserIdAsync(userId);
            if (cart == null)
                return ApiResponse<bool>.ErrorResult("Cart not found");

            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;
            await _uow.Carts.UpdateAsync(cart.Id.ToString(), cart);
            await _uow.CommitAsync();

            return ApiResponse<bool>.SuccessResult(true, "Cart cleared");
        }

        // Map Cart entity sang DTO
        private CartDto MapToCartDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id.ToString(),
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items?.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    ImageUrl = i.ImageUrl,
                    Model3DUrl = i.Model3DUrl
                }).ToList() ?? new List<CartItemDto>()
            };
        }
    }
}
