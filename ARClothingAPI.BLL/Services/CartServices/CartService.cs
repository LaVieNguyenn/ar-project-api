using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.DAL.UnitOfWorks;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ARClothingAPI.BLL.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartDto?> GetCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            return cart == null ? null : MapToDto(cart);
        }

        public async Task<CartDto> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
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
                existingItem.Quantity += request.Quantity;
            else
            {
               var product = _unitOfWork.Products.GetByIdAsync(request.ProductId);
                if(product != null)
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = request.ProductId,
                        ProductName = product.Result.Name, // Gán tên sản phẩm từ ProductRepository nếu cần
                        Price = product.Result.Price,               // Gán giá sản phẩm từ ProductRepository nếu cần
                        Quantity = request.Quantity,
                        ImageUrl = product.Result.Images.FirstOrDefault(),
                        Model3DUrl = product.Result.Model3DUrl
                    });
                }
            }
            cart.UpdatedAt = DateTime.UtcNow;

            if (cart.Id == default)
                await _unitOfWork.Carts.InsertAsync(cart);
            else
                await _unitOfWork.Carts.UpdateAsync(cart.Id.ToString(), cart);

            await _unitOfWork.CommitAsync();
            return MapToDto(cart);
        }

        public async Task<CartDto?> UpdateCartItemAsync(string userId, UpdateCartItemRequest request)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart == null) return null;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null) return null;

            item.Quantity = request.Quantity;
            cart.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Carts.UpdateAsync(cart.Id.ToString(), cart);
            await _unitOfWork.CommitAsync();

            return MapToDto(cart);
        }

        public async Task<bool> RemoveCartItemAsync(string userId, string productId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart == null) return false;

            cart.Items.RemoveAll(i => i.ProductId == productId);
            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart.Id.ToString(), cart);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart == null) return false;

            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart.Id.ToString(), cart);
            await _unitOfWork.CommitAsync();

            return true;
        }

        private CartDto MapToDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id.ToString(),
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.ImageUrl,
                    Model3DUrl = i.Model3DUrl
                }).ToList()
            };
        }
    }
}
