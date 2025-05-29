# Cấu hình Google Drive cho AR Clothing API

## Tổng quan
AR Clothing API đã được cập nhật để sử dụng Google Drive làm nơi lưu trữ ảnh sản phẩm. Điều này giúp tiết kiệm không gian lưu trữ trên server và tận dụng khả năng lưu trữ đám mây của Google Drive.

## Các thay đổi đã thực hiện
1. Thêm dịch vụ `GoogleDriveService` để tương tác với Google Drive API
2. Cập nhật `ProductService` để sử dụng Google Drive cho việc lưu trữ ảnh
3. Thêm API endpoints mới để upload và xóa ảnh sản phẩm

## Các API mới
1. **Upload một ảnh sản phẩm**
   - Endpoint: `POST /api/v1/product/{id}/images`
   - Yêu cầu: Multipart form-data với một file ảnh
   - Quyền: Admin

2. **Upload nhiều ảnh sản phẩm**
   - Endpoint: `POST /api/v1/product/{id}/images/multiple`
   - Yêu cầu: Multipart form-data với nhiều file ảnh 
   - Quyền: Admin
   - Ghi chú: API này cho phép upload nhiều ảnh cùng lúc và trả về thông tin chi tiết về số lượng ảnh upload thành công/thất bại

3. **Xóa ảnh sản phẩm**
   - Endpoint: `DELETE /api/v1/product/{id}/images?imageUrl={url}`
   - Yêu cầu: URL của ảnh cần xóa
   - Quyền: Admin
   
4. **Xóa tất cả ảnh khi xóa sản phẩm**
   - Khi gọi API xóa sản phẩm `DELETE /api/v1/product/{id}`, tất cả ảnh liên quan sẽ tự động bị xóa khỏi Google Drive

## Cấu hình Google Drive

### 1. Tạo Google Cloud Project
1. Truy cập [Google Cloud Console](https://console.cloud.google.com/)
2. Tạo một project mới
3. Bật Google Drive API cho project

### 2. Tạo Service Account
1. Trong Google Cloud Console, chọn project của bạn
2. Điều hướng đến "IAM & Admin" > "Service Accounts"
3. Nhấp vào "Create Service Account"
4. Nhập tên và mô tả cho service account
5. Cấp vai trò "Editor" cho service account
6. Tạo một khóa mới cho service account, chọn định dạng JSON
7. Tải xuống file JSON chứa thông tin xác thực

### 3. Cấu hình ARClothingAPI
1. Đổi tên file JSON đã tải về thành `credentials.json`
2. **Vị trí file credentials.json**: Có nhiều vị trí có thể đặt file này (theo thứ tự ưu tiên):
   - Thư mục gốc của project (`f:/Dev_Maui/ar-project-api/credentials.json`)
   - Thư mục bin khi build (`f:/Dev_Maui/ar-project-api/ARClothingAPI/bin/Debug/net8.0/credentials.json`)
   
3. Cập nhật cấu hình trong `appsettings.json`:
   ```json
   "GoogleDrive": {
     "CredentialsFile": "credentials.json",
     "ApplicationName": "AR Clothing API",
     "UserEmail": "email-của-service-account@...iam.gserviceaccount.com"
   }
   ```
   Lưu ý: `UserEmail` phải là email của service account (được tìm thấy trong file credentials.json - client_email), không phải email của MyFit.

4. **Điều quan trọng**: User thực thi ứng dụng phải có quyền đọc file `credentials.json`. Nếu chạy ứng dụng dưới quyền admin nhưng file được tạo bởi user khác, có thể gặp lỗi "File not found" dù file thực tế tồn tại.

### 4. Cấp quyền cho Service Account
1. Đăng nhập vào Google Drive bằng tài khoản Gmail của MyFit
2. Tạo một thư mục để lưu trữ ảnh sản phẩm (nếu bạn muốn lưu ảnh vào một thư mục cụ thể)
3. Chia sẻ thư mục với service account email (được tìm thấy trong file credentials.json)
4. Cấp quyền "Editor" cho service account

## Chú ý
- Khi tải ảnh lên thông qua API, ảnh sẽ được lưu trữ trong Google Drive và API sẽ trả về URL công khai
- URL công khai có định dạng: `https://drive.google.com/uc?export=view&id=FILE_ID`
- API tự động cấp quyền công khai cho các file được tải lên để có thể truy cập mà không cần xác thực
- Khi xóa sản phẩm, tất cả các ảnh liên quan sẽ bị xóa khỏi Google Drive

## Kiểm tra hệ thống
Để kiểm tra hệ thống sau khi thiết lập:
1. Đăng nhập vào hệ thống với tư cách Admin
2. Tạo một sản phẩm mới
3. Sử dụng API upload ảnh để tải một ảnh cho sản phẩm
4. Kiểm tra xem ảnh có xuất hiện trong Google Drive không
5. Kiểm tra xem URL trả về có truy cập được không

## Xử lý lỗi
Nếu gặp lỗi khi tải ảnh lên hoặc xóa ảnh, vui lòng kiểm tra:
- File `credentials.json` đã được đặt đúng vị trí
- Email trong cấu hình `appsettings.json` đã được cấp quyền truy cập vào Google Drive
- Service account đã được cấp quyền đầy đủ
