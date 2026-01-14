# 📰 NetNews - Hệ thống Quản trị Tin Tức Đa Phương Tiện

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat&logo=bootstrap)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019%2B-CC2927?style=flat&logo=microsoft-sql-server)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

**NetNews** là một hệ thống website tin tức (CMS) hoàn chỉnh, được thiết kế để cung cấp trải nghiệm đọc tin tức tối ưu cho người dùng và giao diện quản trị mạnh mẽ cho biên tập viên. Dự án được xây dựng theo mô hình **MVC (Model-View-Controller)** sử dụng công nghệ mới nhất từ Microsoft là **ASP.NET Core 9**.

## 🚀 Tính năng chính

### 👤 Phân hệ Người dùng (Front-end)
* **Trang chủ hiện đại:** Hiển thị tin nổi bật, tin mới nhất theo bố cục lưới (Grid system).
* **Phân loại thông minh:** Duyệt tin tức theo Danh mục (Thế giới, Công nghệ, Thể thao...).
* **Chi tiết bài viết:** Hỗ trợ hiển thị nội dung Rich Text, hình ảnh, video.
* **Tương tác:** Bình luận, tìm kiếm bài viết.
* **Responsive:** Giao diện tương thích hoàn toàn trên Mobile, Tablet và Desktop (Bootstrap 5).

### 🛡️ Phân hệ Quản trị (Admin Area)
* **Dashboard:** Thống kê tổng quan số lượng bài viết, lượt xem.
* **Quản lý Danh mục:** Thêm, sửa, xóa, ẩn/hiện danh mục tin.
* **Quản lý Bài viết:** Soạn thảo bài viết với trình editor chuyên nghiệp (CKEditor/TinyMCE), upload ảnh cover.
* **Phân quyền (RBAC):** Hệ thống phân quyền chặt chẽ (Admin, Editor, Member) sử dụng ASP.NET Core Identity.
* **Quản lý Media:** Quản lý hình ảnh và tài nguyên tập trung.

## 🛠️ Công nghệ sử dụng

* **Backend:** ASP.NET Core 9.0 MVC
* **Database:** SQL Server, Entity Framework Core (Code First)
* **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript (jQuery)
* **Authentication:** ASP.NET Core Identity
* **Tools:** Visual Studio 2022, SQL Server Management Studio (SSMS)

## ⚙️ Cài đặt và Hướng dẫn chạy

Để chạy dự án này trên máy cục bộ, vui lòng làm theo các bước sau:

1.  **Clone repository:**
    ```bash
    git clone https://github.com/manhtuan28/NetNews.git
    ```
2.  **Cấu hình Cơ sở dữ liệu:**
    * Mở file `appsettings.json`.
    * Điều chỉnh chuỗi kết nối `DefaultConnection` phù hợp với SQL Server của bạn.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=NetNews;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
    }
    ```
3.  **Khởi tạo Database (Migration):**
    * Mở **Package Manager Console** trong Visual Studio.
    * Chạy lệnh:
    ```powershell
    Update-Database
    ```
4.  **Chạy ứng dụng:**
    * Nhấn `F5` hoặc `Ctrl + F5` trong Visual Studio để khởi chạy.

## 🤝 Đóng góp
Mọi ý kiến đóng góp hoặc báo lỗi (Issue) đều được hoan nghênh. Vui lòng tạo Pull Request nếu bạn muốn tham gia phát triển.

## 📝 License
Dự án được phát hành dưới giấy phép MIT.

---
*Developed by [Manh Tuan](https://github.com/manhtuan28)*