namespace ARClothingAPI.Common.DTOs
{
    public class GoogleDriveFileDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string WebViewLink { get; set; } = null!;
        public string? ThumbnailLink { get; set; }
    }

    public class UploadFileRequest
    {
        public string FileName { get; set; } = null!;
        public string FileContent { get; set; } = null!; // Base64 string
        public string MimeType { get; set; } = null!;
        public string? FolderId { get; set; }
    }
}
