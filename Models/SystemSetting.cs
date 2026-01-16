using System.ComponentModel.DataAnnotations;

namespace NetNews.Models
{
    public class SystemSetting
    {
        public int Id { get; set; }
        public string WebsiteName { get; set; }
        public string Slogan { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string FooterText { get; set; }
        public string FacebookUrl { get; set; }
        public string YoutubeUrl { get; set; }
    }
}