using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;

namespace ARClothingAPI.BLL.Services.UserServices
{
    public interface IUserService
    {
        Task<ApiResponse<UserDetailDto>> GetUserByIdAsync(string id);
        Task<ApiResponse<UserDetailDto>> UpdateUserAsync(string id, UserUpdateDto userDto);
        Task<ApiResponse<bool>> ChangePasswordAsync(string id, UserChangePasswordDto passwordDto);
    }
}
