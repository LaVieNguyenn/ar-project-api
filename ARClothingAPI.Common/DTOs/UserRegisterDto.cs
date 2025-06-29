namespace ARClothingAPI.Common.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public string Password { get; set; } = null!;
        public int Gender { get; set; }
        public string RoleId { get; set; } = null!;
        public double Height { get; set; }
        public double Weight { get; set; }
    }
}
