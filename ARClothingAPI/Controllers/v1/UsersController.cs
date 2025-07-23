using ARClothingAPI.BLL.Services.UserServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetUserDetail(string id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetCurrentUser()
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null)
                return Unauthorized(ApiResponse<UserDetailDto>.ErrorResult("User not authenticated"));

            return await _userService.GetUserByIdAsync(userId);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> UpdateUser(string id, [FromBody] UserUpdateDto userDto)
        {
            var userId = User.FindFirst("id")?.Value;

            // Only allow users to update their own profile unless they are admin
            if (userId != id && !User.IsInRole("Admin"))
                return Unauthorized(ApiResponse<UserDetailDto>.ErrorResult("You are not authorized to update this user"));

            return await _userService.UpdateUserAsync(id, userDto);
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDetailDto>>> UpdateCurrentUser([FromBody] UserUpdateDto userDto)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null)
                return Unauthorized(ApiResponse<UserDetailDto>.ErrorResult("User not authenticated"));

            return await _userService.UpdateUserAsync(userId, userDto);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] UserChangePasswordDto passwordDto)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null)
                return Unauthorized(ApiResponse<bool>.ErrorResult("User not authenticated"));

            return await _userService.ChangePasswordAsync(userId, passwordDto);
        }
    }
}
