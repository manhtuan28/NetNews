namespace NetNews.Areas.Admin.Models
{
    public class CommentManagementViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string PostTitle { get; set; }
        public string PostSlug { get; set; }
        public bool IsReply { get; set; }
    }
}