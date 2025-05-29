namespace ARClothingAPI.Common.DTOs
{
    public class UserUpdateDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
    }
}
