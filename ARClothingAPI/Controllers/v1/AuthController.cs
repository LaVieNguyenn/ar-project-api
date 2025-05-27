using ARClothingAPI.BLL.Services.AuthServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace ARClothingAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] UserRegisterDto req)
            => await _authService.RegisterAsync(req);

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] UserLoginDto req)
            => await _authService.LoginAsync(req);
    }

}
