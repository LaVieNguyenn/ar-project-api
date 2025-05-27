using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Helpers;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;
using Microsoft.Extensions.Configuration;

namespace ARClothingAPI.BLL.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly string _jwtSecret;
        private readonly int _jwtExpiryMinutes;

        public AuthService(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _jwtSecret = config["Jwt:Secret"]!;
            _jwtExpiryMinutes = int.Parse(config["Jwt:ExpiryMinutes"] ?? "120");
        }

        public async Task<ApiResponse<string>> RegisterAsync(UserRegisterDto req)
        {
            if (await _uow.Users.ExistsByEmailAsync(req.Email))
                return ApiResponse<string>.ErrorResult("Email already exists");

            var user = new User
            {
                Username = req.Username,
                Email = req.Email.ToLower(),
                PasswordHash = PasswordHasher.Hash(req.Password),
                RoleId = req.RoleId,
                Gender = req.Gender,
                Height = req.Height,
                Weight = req.Weight,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _uow.Users.InsertAsync(user);
            await _uow.CommitAsync();
            return ApiResponse<string>.SuccessResult(user.Id, "Register successful");
        }

        public async Task<ApiResponse<string>> LoginAsync(UserLoginDto req)
        {
            var user = await _uow.Users.GetByEmailAsync(req.Email.ToLower());
            if (user == null || !PasswordHasher.Verify(req.Password, user.PasswordHash) || !user.IsActive)
                return ApiResponse<string>.ErrorResult("Invalid email or password");

            var token = JwtHelper.GenerateJwtToken(user, _jwtSecret, _jwtExpiryMinutes);
            return ApiResponse<string>.SuccessResult(token, "Login successful");
        }
    }

}
