using ARClothingAPI.Common.DTOs;
using ARClothingAPI.Common.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ARClothingAPI.BLL.Services.GoogleDriveServices
{
    public interface IGoogleDriveService
    {
        /// <summary>
        /// Upload một file lên Google Drive
        /// </summary>
        Task<ApiResponse<GoogleDriveFileDto>> UploadFileAsync(IFormFile file, string? folderId = null);

        /// <summary>
        /// Upload một file lên Google Drive từ base64 string
        /// </summary>
        Task<ApiResponse<GoogleDriveFileDto>> UploadFileFromBase64Async(UploadFileRequest request);
        
        /// <summary>
        /// Upload nhiều file lên Google Drive song song
        /// </summary>
        /// <param name="files">Danh sách các file cần upload</param>
        /// <param name="folderId">ID của thư mục chứa file (optional)</param>
        /// <returns>Danh sách kết quả upload của từng file</returns>
        Task<ApiResponse<List<GoogleDriveFileDto>>> UploadMultipleFilesAsync(IList<IFormFile> files, string? folderId = null);

        /// <summary>
        /// Tạo URL công khai từ Google Drive file ID
        /// </summary>
        string GetPublicUrl(string fileId);

        /// <summary>
        /// Xóa file khỏi Google Drive
        /// </summary>
        Task<ApiResponse<bool>> DeleteFileAsync(string fileId);

        /// <summary>
        /// Lấy thông tin file từ Google Drive
        /// </summary>
        Task<ApiResponse<GoogleDriveFileDto>> GetFileAsync(string fileId);
    }
}
