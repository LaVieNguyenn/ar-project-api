namespace ARClothingAPI.Common.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? Model3DUrl { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
        public string CategoryId { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
