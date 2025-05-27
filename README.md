# AR Clothing API

## 🌟 Mục tiêu dự án

Xây dựng một hệ thống API **đa nền tảng, hiện đại** phục vụ ứng dụng AR thử quần áo thực tế ảo trên thiết bị di động/web.  
API cung cấp đầy đủ chức năng cho user authentication, quản lý tài khoản, lưu trữ sản phẩm/quần áo, hỗ trợ kết nối các ứng dụng AR (Unity/MAUI) với backend bảo mật cao, tối ưu hiệu năng và dễ dàng mở rộng.

---

## 🎯 Mục đích sử dụng

- **Đăng ký, đăng nhập, xác thực người dùng** qua JWT.
- **Quản lý dữ liệu người dùng** (profile, kích thước cơ thể, quyền hạn, v.v).
- **Lưu trữ, truy xuất sản phẩm/quần áo** (3D Model, hình ảnh, size, thông tin chi tiết...).
- **Tích hợp với hệ thống thử đồ thực tế ảo (AR Fitting Room)**: cho phép người dùng chọn quần áo và thử trên người qua camera điện thoại.
- Hỗ trợ **quản trị viên (Admin)** kiểm soát dữ liệu, duyệt/quản lý sản phẩm.
- **Dễ dàng tích hợp với mobile app, web app, Unity/MAUI front-end**.

---

## ⚙️ Công nghệ sử dụng

- **.NET 8 Web API**: RESTful, versioning.
- **MongoDB**: NoSQL Database, lưu trữ dữ liệu người dùng, sản phẩm, phân tách nhiều database (Auth & Storage).
- **JWT Authentication**: xác thực và phân quyền bảo mật, chuẩn cho mobile/frontend.
- **Dependency Injection (DI)**: chia layer rõ ràng, dễ test, dễ maintain.
- **3 Layer Architecture**:
    - **Presentation (API)**
    - **Business Logic Layer (BLL)**
    - **Data Access Layer (DAL)**
    - **Common**: DTOs, helpers, model, constants.
- **Unit of Work & Generic Repository Pattern**: tối ưu code, dễ scale các loại entity/data.
- **Swagger/OpenAPI**: test API nhanh, tài liệu tự động.
- **Docker-ready**: sẵn sàng triển khai production/dev bằng container.

---

## 🗂️ Cấu trúc Solution

```

/ARClothing.API            // Web API project (Presentation)
/ARClothing.BLL            // Business Logic Layer
/ARClothing.DAL            // Data Access Layer (Repository, UnitOfWork)
/ARClothing.Common         // DTOs, Helpers, Constants, BaseEntity...
/ARClothing.http           // File test REST API nhanh (Register, Login, JWT...)
/README.md

````

---

## 🚀 Hướng dẫn chạy nhanh (Local)

1. **Clone repo:**
   ```bash
   git clone https://github.com/your-org/ar-clothing-api.git
````

2. **Restore package & build solution:**

   ```bash
   dotnet restore
   dotnet build
   ```

3. **Chạy API (mặc định port 5000):**

   ```bash
   dotnet run --project ARClothing.API
   ```

4. **Test nhanh qua file ArClothing.http hoặc Swagger UI.**




