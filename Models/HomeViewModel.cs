using NetNews.Models;

namespace NetNews.Models
{
    public class HomeViewModel
    {
        public List<Post> FeaturedPosts { get; set; }
        public List<Post> RecentPosts { get; set; }
        public List<Post> MostViewedPosts { get; set; }
    }
}