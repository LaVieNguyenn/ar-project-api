using ARClothingAPI.BLL.Services.GoogleDriveServices;
using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Entities;
using ARClothingAPI.Common.Response;
using ARClothingAPI.DAL.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace ARClothingAPI.BLL.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGoogleDriveService _googleDriveService;

        public ProductService(IUnitOfWork uow, IGoogleDriveService googleDriveService)
        {
            _uow = uow;
            _googleDriveService = googleDriveService;
        }

        public async Task<ApiResponse<ProductDto>> CreateAsync(ProductCreateUpdateDto productDto, string userId)
        {
            // Check if category exists
            var category = await _uow.Categories.GetByIdAsync(productDto.CategoryId);
            if (category == null)
                return ApiResponse<ProductDto>.ErrorResult("Category not found");

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Images = productDto.Images,
                Model3DUrl = productDto.Model3DUrl,
                Sizes = productDto.Sizes,
                CategoryId = productDto.CategoryId,
                CreatedBy = userId,
                IsActive = productDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _uow.Products.InsertAsync(product);
            await _uow.CommitAsync();

            var productResponse = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Model3DUrl = product.Model3DUrl,
                Sizes = product.Sizes,
                CategoryId = product.CategoryId,
                CreatedBy = product.CreatedBy,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return ApiResponse<ProductDto>.SuccessResult(productResponse, "Product created successfully");
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllAsync()
        {
            var products = await _uow.Products.GetAllAsync();
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Images = p.Images,
                Model3DUrl = p.Model3DUrl,
                Sizes = p.Sizes,
                CategoryId = p.CategoryId,
                CreatedBy = p.CreatedBy,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });

            return ApiResponse<IEnumerable<ProductDto>>.SuccessResult(productDtos);
        }

        public async Task<ApiResponse<ProductDto>> GetByIdAsync(string id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResult("Product not found");

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Model3DUrl = product.Model3DUrl,
                Sizes = product.Sizes,
                CategoryId = product.CategoryId,
                CreatedBy = product.CreatedBy,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return ApiResponse<ProductDto>.SuccessResult(productDto);
        }

        public async Task<ApiResponse<ProductDto>> UpdateAsync(string id, ProductCreateUpdateDto productDto)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResult("Product not found");

            // Check if category exists if it's changed
            if (productDto.CategoryId != product.CategoryId)
            {
                var category = await _uow.Categories.GetByIdAsync(productDto.CategoryId);
                if (category == null)
                    return ApiResponse<ProductDto>.ErrorResult("Category not found");
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Images = productDto.Images;
            product.Model3DUrl = productDto.Model3DUrl;
            product.Sizes = productDto.Sizes;
            product.CategoryId = productDto.CategoryId;
            product.IsActive = productDto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _uow.Products.UpdateAsync(id, product);
            await _uow.CommitAsync();

            var updatedProductDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Model3DUrl = product.Model3DUrl,
                Sizes = product.Sizes,
                CategoryId = product.CategoryId,
                CreatedBy = product.CreatedBy,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return ApiResponse<ProductDto>.SuccessResult(updatedProductDto, "Product updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
                return ApiResponse<bool>.ErrorResult("Product not found");

            // Xóa tất cả các hình ảnh sản phẩm trên Google Drive
            if (product.Images != null && product.Images.Count > 0)
            {
                foreach (var imageUrl in product.Images)
                {
                    // Lấy fileId từ URL
                    string fileId = GetFileIdFromUrl(imageUrl);
                    if (!string.IsNullOrEmpty(fileId))
                    {
                        await _googleDriveService.DeleteFileAsync(fileId);
                    }
                }
            }

            await _uow.Products.DeleteAsync(id);
            await _uow.CommitAsync();

            return ApiResponse<bool>.SuccessResult(true, "Product deleted successfully");
        }

        public async Task<ApiResponse<ProductDto>> UploadImageAsync(string productId, IFormFile file)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResult("Product not found");

            // Upload file lên Google Drive
            var uploadResult = await _googleDriveService.UploadFileAsync(file);
            if (!uploadResult.Success)
                return ApiResponse<ProductDto>.ErrorResult(uploadResult.Message);

            // Lấy URL công khai để lưu vào product
            string imageUrl = _googleDriveService.GetPublicUrl(uploadResult.Data.Id);

            // Thêm URL vào danh sách Images
            if (product.Images == null)
                product.Images = new List<string>();

            product.Images.Add(imageUrl);
            product.UpdatedAt = DateTime.UtcNow;

            // Cập nhật sản phẩm
            await _uow.Products.UpdateAsync(productId, product);
            await _uow.CommitAsync();

            // Map entity sang DTO để trả về
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Model3DUrl = product.Model3DUrl,
                Sizes = product.Sizes,
                CategoryId = product.CategoryId,
                CreatedBy = product.CreatedBy,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return ApiResponse<ProductDto>.SuccessResult(productDto, "Image uploaded successfully");
        }

        public async Task<ApiResponse<ProductDto>> UploadMultipleImagesAsync(string productId, List<IFormFile> files)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResult("Product not found");

            if (files == null || !files.Any())
                return ApiResponse<ProductDto>.ErrorResult("No files were uploaded");

            // Khởi tạo Images nếu chưa có
            if (product.Images == null)
                product.Images = new List<string>();

            // Lọc các file không phải image
            var imageFiles = new List<IFormFile>();
            var errorMessages = new List<string>();
            
            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    errorMessages.Add($"Skipped empty file");
                    continue;
                }

                // Kiểm tra file type
                if (!file.ContentType.StartsWith("image/"))
                {
                    errorMessages.Add($"Skipped non-image file: {file.FileName}");
                    continue;
                }
                
                imageFiles.Add(file);
            }
            
            if (!imageFiles.Any())
            {
                return ApiResponse<ProductDto>.ErrorResult($"No valid image files were found. Errors: {string.Join("; ", errorMessages)}");
            }

            // Upload nhiều ảnh song song sử dụng Task.WhenAll
            var uploadResult = await _googleDriveService.UploadMultipleFilesAsync(imageFiles);
            
            // Xử lý kết quả upload
            int successCount = 0;
            
            if (uploadResult.Success && uploadResult.Data != null && uploadResult.Data.Any())
            {
                // Thêm các URL hình ảnh vào sản phẩm
                foreach (var fileDto in uploadResult.Data)
                {
                    string imageUrl = _googleDriveService.GetPublicUrl(fileDto.Id);
                    product.Images.Add(imageUrl);
                    successCount++;
                }
                
                // Cập nhật sản phẩm nếu có ít nhất 1 ảnh được upload thành công
                if (successCount > 0)
                {
                    product.UpdatedAt = DateTime.UtcNow;
                    await _uow.Products.UpdateAsync(productId, product);
                    await _uow.CommitAsync();
                }
            }
            else if (!string.IsNullOrEmpty(uploadResult.Message))
            {
                errorMessages.Add(uploadResult.Message);
            }

            // Map entity sang DTO để trả về
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Model3DUrl = product.Model3DUrl,
                Sizes = product.Sizes,
                CategoryId = product.CategoryId,
                CreatedBy = product.CreatedBy,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            string resultMessage = $"{successCount} images uploaded successfully";
            if (errorMessages.Any())
                resultMessage += $". Errors: {string.Join("; ", errorMessages)}";

            return ApiResponse<ProductDto>.SuccessResult(productDto, resultMessage);
        }

        public async Task<ApiResponse<ProductDto>> DeleteImageAsync(string productId, string imageUrl)
        {
            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null)
                return ApiResponse<ProductDto>.ErrorResult("Product not found");

            // Kiểm tra xem URL có trong danh sách Images không
            if (product.Images == null || !product.Images.Contains(imageUrl))
                return ApiResponse<ProductDto>.ErrorResult("Image URL not found in product");

            // Lấy fileId từ URL
            string fileId = GetFileIdFromUrl(imageUrl);
            if (string.IsNullOrEmpty(fileId))
                return ApiResponse<ProductDto>.ErrorResult("Invalid image URL");

            // Xóa file trên Google Drive
            var deleteResult = await _googleDriveService.DeleteFileAsync(fileId);
            if (!deleteResult.Success)
                return ApiResponse<ProductDto>.ErrorResult(deleteResult.Message);

            // Xóa URL khỏi danh sách Images
            product.Images.Remove(imageUrl);
            product.UpdatedAt = DateTime.UtcNow;

            // Cập nhật sản phẩm
            await _uow.Products.UpdateAsync(productId, product);
            await _uow.CommitAsync();

            // Map entity sang DTO để trả về
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Model3DUrl = product.Model3DUrl,
                Sizes = product.Sizes,
                CategoryId = product.CategoryId,
                CreatedBy = product.CreatedBy,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            return ApiResponse<ProductDto>.SuccessResult(productDto, "Image deleted successfully");
        }

        // Phương thức trích xuất fileId từ URL
        private string GetFileIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            // URL có dạng: https://drive.google.com/uc?export=view&id=FILE_ID
            if (url.Contains("id="))
            {
                var fileId = url.Split("id=").Last();
                return fileId;
            }

            return string.Empty;
        }
    }
}
