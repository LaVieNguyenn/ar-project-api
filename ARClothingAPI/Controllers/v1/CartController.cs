using ARClothingAPI.BLL.Services.CartServices;
using ARClothingAPI.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("id");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetCartAsync(GetUserId());
            if (cart == null)
                return NotFound(new { message = "Cart not found." });
            return Ok(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var cart = await _cartService.AddToCartAsync(GetUserId(), request);
            return Ok(cart);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            var cart = await _cartService.UpdateCartItemAsync(GetUserId(), request);
            if (cart == null) return NotFound(new { message = "Cart or item not found." });
            return Ok(cart);
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCartItem(string productId)
        {
            var success = await _cartService.RemoveCartItemAsync(GetUserId(), productId);
            if (!success) return NotFound(new { message = "Cart or item not found." });
            return Ok(new { message = "Removed item from cart." });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var success = await _cartService.ClearCartAsync(GetUserId());
            if (!success) return NotFound(new { message = "Cart not found." });
            return Ok(new { message = "Cart cleared." });
        }
    }
}
