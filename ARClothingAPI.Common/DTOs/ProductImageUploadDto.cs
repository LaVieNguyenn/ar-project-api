using Microsoft.AspNetCore.Http;

namespace ARClothingAPI.Common.DTOs
{
    public class ProductImageUploadDto
    {
        public string ProductId { get; set; } = null!;
        public IFormFile File { get; set; } = null!;
    }
    
    public class ProductImagesUploadDto
    {
        public string ProductId { get; set; } = null!;
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
