using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ARClothingAPI.BLL.Services.GoogleDriveServices
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly IConfiguration _configuration;

        public GoogleDriveService(IConfiguration configuration)
        {
            _configuration = configuration;
            _driveService = InitializeDriveService();
        }

        private DriveService InitializeDriveService()
        {
            try
            {
                var credentialsJson = _configuration["GoogleDrive:CredentialsFile"];
                var credentialsFilePath = _configuration["GoogleDrive:CredentialsFile"];
                var applicationName = _configuration["GoogleDrive:ApplicationName"];
                var userEmail = _configuration["GoogleDrive:UserEmail"];

                if (string.IsNullOrEmpty(applicationName))
                    throw new InvalidOperationException("GoogleDrive:ApplicationName configuration is missing");

                if (string.IsNullOrEmpty(userEmail))
                    throw new InvalidOperationException("GoogleDrive:UserEmail configuration is missing");

                GoogleCredential credential = null;

                // Ưu tiên đọc từ biến môi trường (env/config) nếu có
                if (!string.IsNullOrWhiteSpace(credentialsJson))
                {
                    // Đọc từ JSON chuỗi
                    using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialsJson)))
                    {
                        credential = GoogleCredential.FromStream(stream)
                            .CreateScoped(DriveService.ScopeConstants.Drive)
                            .CreateWithUser(userEmail);
                    }
                }
                else
                {
                    // Fallback: Đọc từ file vật lý (dev/local)
                    if (string.IsNullOrEmpty(credentialsFilePath))
                        throw new InvalidOperationException("GoogleDrive:CredentialsFile configuration is missing and no CredentialsJson provided");

                    // Convert to absolute path nếu là đường dẫn tương đối
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string absolutePath = System.IO.Path.Combine(baseDir, credentialsFilePath);
                    if (!System.IO.File.Exists(absolutePath))
                        throw new FileNotFoundException($"Google credentials file not found at path: {absolutePath}");

                    using (var stream = new System.IO.FileStream(absolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        credential = GoogleCredential.FromStream(stream)
                            .CreateScoped(DriveService.ScopeConstants.Drive)
                            .CreateWithUser(userEmail);
                    }
                }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = applicationName,
                });

                return service;
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error initializing Google Drive service: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null; // Trả về null hoặc có thể throw exception tùy theo yêu cầu
                //throw;
            }
        }

        public async Task<ApiResponse<GoogleDriveFileDto>> UploadFileAsync(IFormFile file, string? folderId = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return ApiResponse<GoogleDriveFileDto>.ErrorResult("No file was uploaded");

                using (var stream = file.OpenReadStream())
                {
                    // Xác định MIME type từ file
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = file.FileName,
                        MimeType = file.ContentType,
                        Parents = folderId != null ? new List<string> { folderId } : null
                    };

                    // Tạo upload request
                    var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                    request.Fields = "id, name, mimeType, webViewLink, thumbnailLink";

                    // Upload file lên Google Drive
                    var uploadProgress = await request.UploadAsync();

                    if (uploadProgress.Status != UploadStatus.Completed)
                    {
                        var errorDetails = uploadProgress.Exception?.ToString() ?? "Unknown error";
                        Console.WriteLine($"Google Drive Upload Error: {errorDetails}");
                        return ApiResponse<GoogleDriveFileDto>.ErrorResult($"Upload failed: {uploadProgress.Exception?.Message}. Status: {uploadProgress.Status}. Exception details: {errorDetails}");
                    }

                    // Lấy thông tin file đã upload
                    var uploadedFile = request.ResponseBody;

                    // Cấp quyền cho file để có thể truy cập công khai
                    await SetFilePublicPermissionAsync(uploadedFile.Id);

                    // Map sang DTO
                    var fileDto = new GoogleDriveFileDto
                    {
                        Id = uploadedFile.Id,
                        Name = uploadedFile.Name,
                        MimeType = uploadedFile.MimeType,
                        WebViewLink = uploadedFile.WebViewLink,
                        ThumbnailLink = uploadedFile.ThumbnailLink
                    };

                    return ApiResponse<GoogleDriveFileDto>.SuccessResult(fileDto, "File uploaded successfully");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<GoogleDriveFileDto>.ErrorResult($"Error uploading file: {ex.Message}");
            }
        }

        public async Task<ApiResponse<GoogleDriveFileDto>> UploadFileFromBase64Async(UploadFileRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FileContent))
                    return ApiResponse<GoogleDriveFileDto>.ErrorResult("No file content was provided");

                // Decode base64 string to byte array
                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(request.FileContent);
                }
                catch
                {
                    return ApiResponse<GoogleDriveFileDto>.ErrorResult("Invalid base64 string");
                }

                using (var stream = new System.IO.MemoryStream(fileBytes))
                {
                    // Tạo metadata cho file
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = request.FileName,
                        MimeType = request.MimeType,
                        Parents = request.FolderId != null ? new List<string> { request.FolderId } : null
                    };

                    // Tạo upload request
                    var uploadRequest = _driveService.Files.Create(fileMetadata, stream, request.MimeType);
                    uploadRequest.Fields = "id, name, mimeType, webViewLink, thumbnailLink";

                    // Upload file
                    var uploadProgress = await uploadRequest.UploadAsync();

                    if (uploadProgress.Status != UploadStatus.Completed)
                    {
                        var errorDetails = uploadProgress.Exception?.ToString() ?? "Unknown error";
                        Console.WriteLine($"Google Drive Upload Error (Base64): {errorDetails}");
                        return ApiResponse<GoogleDriveFileDto>.ErrorResult($"Upload failed: {uploadProgress.Exception?.Message}. Status: {uploadProgress.Status}. Exception details: {errorDetails}");
                    }

                    // Lấy thông tin file đã upload
                    var uploadedFile = uploadRequest.ResponseBody;

                    // Cấp quyền cho file để có thể truy cập công khai
                    await SetFilePublicPermissionAsync(uploadedFile.Id);

                    // Map sang DTO
                    var fileDto = new GoogleDriveFileDto
                    {
                        Id = uploadedFile.Id,
                        Name = uploadedFile.Name,
                        MimeType = uploadedFile.MimeType,
                        WebViewLink = uploadedFile.WebViewLink,
                        ThumbnailLink = uploadedFile.ThumbnailLink
                    };

                    return ApiResponse<GoogleDriveFileDto>.SuccessResult(fileDto, "File uploaded successfully");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<GoogleDriveFileDto>.ErrorResult($"Error uploading file: {ex.Message}");
            }
        }

        public string GetPublicUrl(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
                return string.Empty;

            return $"https://drive.google.com/uc?export=view&id={fileId}";
        }

        public async Task<ApiResponse<bool>> DeleteFileAsync(string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId))
                    return ApiResponse<bool>.ErrorResult("File ID cannot be empty");

                // Xóa file từ Google Drive
                await _driveService.Files.Delete(fileId).ExecuteAsync();
                return ApiResponse<bool>.SuccessResult(true, "File deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error deleting file: {ex.Message}");
            }
        }

        public async Task<ApiResponse<GoogleDriveFileDto>> GetFileAsync(string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId))
                    return ApiResponse<GoogleDriveFileDto>.ErrorResult("File ID cannot be empty");

                // Lấy thông tin file
                var request = _driveService.Files.Get(fileId);
                request.Fields = "id, name, mimeType, webViewLink, thumbnailLink";
                var file = await request.ExecuteAsync();

                // Map sang DTO
                var fileDto = new GoogleDriveFileDto
                {
                    Id = file.Id,
                    Name = file.Name,
                    MimeType = file.MimeType,
                    WebViewLink = file.WebViewLink,
                    ThumbnailLink = file.ThumbnailLink
                };

                return ApiResponse<GoogleDriveFileDto>.SuccessResult(fileDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<GoogleDriveFileDto>.ErrorResult($"Error retrieving file: {ex.Message}");
            }
        }

        // Phương thức để set quyền public cho file
        private async Task SetFilePublicPermissionAsync(string fileId)
        {
            try
            {
                // Tạo permission để file có thể truy cập công khai
                var permission = new Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };

                // Cấp quyền cho file
                await _driveService.Permissions.Create(permission, fileId).ExecuteAsync();
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error setting public permission for file {fileId}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Không throw exception vì việc set permission không thành công không ảnh hưởng đến việc file đã được upload
            }
        }

        public async Task<ApiResponse<List<GoogleDriveFileDto>>> UploadMultipleFilesAsync(IList<IFormFile> files, string? folderId = null)
        {
            try
            {
                if (files == null || !files.Any())
                    return ApiResponse<List<GoogleDriveFileDto>>.ErrorResult("No files were provided for upload");

                // Tạo danh sách các task để upload từng file
                var uploadTasks = new List<Task<ApiResponse<GoogleDriveFileDto>>>();

                // Thêm từng file vào danh sách task để upload
                foreach (var file in files)
                {
                    uploadTasks.Add(UploadFileAsync(file, folderId));
                }

                // Chạy tất cả các task upload song song và đợi tất cả hoàn thành
                var results = await Task.WhenAll(uploadTasks);

                // Tổng hợp kết quả
                var successfulUploads = new List<GoogleDriveFileDto>();
                var failedCount = 0;
                var errorMessages = new List<string>();

                foreach (var result in results)
                {
                    if (result.Success && result.Data != null)
                    {
                        successfulUploads.Add(result.Data);
                    }
                    else
                    {
                        failedCount++;
                        if (!string.IsNullOrEmpty(result.Message))
                        {
                            errorMessages.Add(result.Message);
                        }
                    }
                }

                // Tổng hợp thông báo
                var message = $"{successfulUploads.Count} files uploaded successfully";
                if (failedCount > 0)
                {
                    message += $", {failedCount} files failed to upload";
                    // Thêm chi tiết lỗi nếu có
                    if (errorMessages.Any())
                    {
                        message += $". Errors: {string.Join("; ", errorMessages)}";
                    }

                    if (successfulUploads.Count == 0)
                    {
                        // Nếu không có file nào upload thành công, trả về error result
                        return ApiResponse<List<GoogleDriveFileDto>>.ErrorResult(message);
                    }
                }

                return ApiResponse<List<GoogleDriveFileDto>>.SuccessResult(successfulUploads, message);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GoogleDriveFileDto>>.ErrorResult($"Error uploading multiple files: {ex.Message}");
            }
        }
    }
}
