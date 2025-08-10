namespace ARClothingAPI.Common.DTOs
{
    public class CategoryDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int? Order { get; set; } = 1;
        public string? ParentCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
