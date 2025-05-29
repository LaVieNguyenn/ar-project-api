using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;

namespace ARClothingAPI.BLL.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ApiResponse<UserDetailDto>> GetUserByIdAsync(string id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            if (user == null)
                return ApiResponse<UserDetailDto>.ErrorResult("User not found");

            var userDto = new UserDetailDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Gender = user.Gender,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                Height = user.Height,
                Weight = user.Weight,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return ApiResponse<UserDetailDto>.SuccessResult(userDto);
        }

        public async Task<ApiResponse<UserDetailDto>> UpdateUserAsync(string id, UserUpdateDto userDto)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            if (user == null)
                return ApiResponse<UserDetailDto>.ErrorResult("User not found");

            if (userDto.Email != user.Email && await _uow.Users.ExistsByEmailAsync(userDto.Email))
                return ApiResponse<UserDetailDto>.ErrorResult("Email already exists");

            user.Username = userDto.Username;
            user.Email = userDto.Email.ToLower();
            user.AvatarUrl = userDto.AvatarUrl;
            user.Gender = userDto.Gender;
            user.Height = userDto.Height;
            user.Weight = userDto.Weight;
            user.UpdatedAt = DateTime.UtcNow;

            await _uow.Users.UpdateAsync(id, user);
            await _uow.CommitAsync();

            var updatedUserDto = new UserDetailDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Gender = user.Gender,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                Height = user.Height,
                Weight = user.Weight,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return ApiResponse<UserDetailDto>.SuccessResult(updatedUserDto, "User updated successfully");
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(string id, UserChangePasswordDto passwordDto)
        {
            if (passwordDto.NewPassword != passwordDto.ConfirmPassword)
                return ApiResponse<bool>.ErrorResult("New password and confirm password do not match");

            var user = await _uow.Users.GetByIdAsync(id);
            if (user == null)
                return ApiResponse<bool>.ErrorResult("User not found");

            if (!PasswordHasher.Verify(passwordDto.CurrentPassword, user.PasswordHash))
                return ApiResponse<bool>.ErrorResult("Current password is incorrect");

            user.PasswordHash = PasswordHasher.Hash(passwordDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _uow.Users.UpdateAsync(id, user);
            await _uow.CommitAsync();

            return ApiResponse<bool>.SuccessResult(true, "Password changed successfully");
        }
    }
}
