using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.BLL.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(UserRegisterDto req);
        Task<ApiResponse<string>> LoginAsync(UserLoginDto req);
    }
}
