using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;

namespace ARClothingAPI.BLL.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(UserRegisterDto req);
        Task<ApiResponse<string>> LoginAsync(UserLoginDto req);
    }
}
