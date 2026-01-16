using NetNews.Models;

namespace NetNews.Models
{
    public class HomeViewModel
    {
        public List<Post> FeaturedPosts { get; set; }
        public List<Post> RecentPosts { get; set; }
        public List<Post> MostViewedPosts { get; set; }
        public List<CategoryBoxViewModel> CategoryBoxes { get; set; }
    }

    public class CategoryBoxViewModel
    {
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public List<Post> Posts { get; set; }
    }
}