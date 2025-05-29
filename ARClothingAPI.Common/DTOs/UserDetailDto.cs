namespace ARClothingAPI.Common.DTOs
{
    public class UserDetailDto
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int Gender { get; set; }
        public string RoleId { get; set; } = null!;
        public bool IsActive { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
