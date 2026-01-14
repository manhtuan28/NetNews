using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace NetNews.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Slug { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<Post>? Posts { get; set; }
    }
}