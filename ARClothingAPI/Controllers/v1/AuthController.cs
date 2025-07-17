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
        {
            var result = await _authService.RegisterAsync(req);
            if(!result.Success) return BadRequest(result.Message);   
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] UserLoginDto req)
        {
            var result = await _authService.LoginAsync(req);
            if (!result.Success) return Unauthorized(result.Message);
            return Ok(result);  
        }
    }

}
