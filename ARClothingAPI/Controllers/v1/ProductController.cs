    using ARClothingAPI.BLL.Services.ProductServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARClothingAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] ProductCreateUpdateDto productDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId == null)
            //    return Unauthorized(ApiResponse<ProductDto>.ErrorResult("User not authenticated"));

            return await _productService.CreateAsync(productDto, userId);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAllProducts(string? categoryId = null, string? productName = null)
        {
            return await _productService.GetAllAsync(categoryId, productName);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProduct(string id)
        {
            return await _productService.GetByIdAsync(id);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(string id, [FromBody] ProductCreateUpdateDto productDto)
        {
            return await _productService.UpdateAsync(id, productDto);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(string id)
        {
            return await _productService.DeleteAsync(id);
        }

        [HttpPost("{id}/images")]
        //[Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> UploadProductImage(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<ProductDto>.ErrorResult("No file was uploaded"));

            // Kiểm tra file có phải là file ảnh không
            if (!file.ContentType.StartsWith("image/"))
                return BadRequest(ApiResponse<ProductDto>.ErrorResult("File must be an image"));

            return await _productService.UploadImageAsync(id, file);
        }

        [HttpPost("{id}/images/multiple")]
        //[Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> UploadMultipleProductImages(string id, [FromForm] List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return BadRequest(ApiResponse<ProductDto>.ErrorResult("No files were uploaded"));

            // Gọi service để xử lý - validate chi tiết sẽ được thực hiện trong service
            return await _productService.UploadMultipleImagesAsync(id, files);
        }

        [HttpDelete("{id}/images")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> DeleteProductImage(string id, [FromQuery] string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return BadRequest(ApiResponse<ProductDto>.ErrorResult("Image URL cannot be empty"));

            return await _productService.DeleteImageAsync(id, imageUrl);
        }
    }
}
