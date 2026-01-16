using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NetNews.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; }

        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int? ParentId { get; set; }
        public Comment Parent { get; set; }
        public List<Comment> Replies { get; set; } = new List<Comment>();
    }
}