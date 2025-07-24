using ARClothingAPI.BLL.Services.CartServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string? GetUserId()
            => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CartDto>>> GetCart()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<CartDto>.ErrorResult("User not authenticated"));
            return await _cartService.GetCartAsync(userId);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse<CartDto>>> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<CartDto>.ErrorResult("User not authenticated"));
            return await _cartService.AddToCartAsync(userId, request);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<CartDto>>> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<CartDto>.ErrorResult("User not authenticated"));
            return await _cartService.UpdateCartItemAsync(userId, request);
        }

        [HttpDelete("remove/{productId}")]
        public async Task<ActionResult<ApiResponse<CartDto>>> RemoveCartItem(string productId)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<CartDto>.ErrorResult("User not authenticated"));
            return await _cartService.RemoveCartItemAsync(userId, productId);
        }

        [HttpDelete("clear")]
        public async Task<ActionResult<ApiResponse<bool>>> ClearCart()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized(ApiResponse<bool>.ErrorResult("User not authenticated"));
            return await _cartService.ClearCartAsync(userId);
        }
    }
}
