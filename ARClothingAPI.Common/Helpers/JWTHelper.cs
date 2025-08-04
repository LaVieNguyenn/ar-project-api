using ARClothingAPI.Common.Entities;
using System.Security.Claims;

namespace ARClothingAPI.Common.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(User user, string secret, int expiryMinutes)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes(secret);

            var claims = new List<System.Security.Claims.Claim>
        {
            new("id", user.Id),
            new("email", user.Email),
            new("username", user.Username),
            new(ClaimTypes.Role, user.RoleId)
        };

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
