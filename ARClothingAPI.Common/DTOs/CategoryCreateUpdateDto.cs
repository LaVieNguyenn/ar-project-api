namespace ARClothingAPI.Common.DTOs
{
    public class CategoryCreateUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ParentCategoryId { get; set; }
    }
}
