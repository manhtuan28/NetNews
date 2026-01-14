using System.ComponentModel.DataAnnotations;

namespace NetNews.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

    }

    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn một vai trò.")]
        public string SelectedRole { get; set; }

        public List<string> AllRoles { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quyền hạn")]
        public string Role { get; set; }
    }
}