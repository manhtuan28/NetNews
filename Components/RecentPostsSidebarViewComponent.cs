using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetNews.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NetNews.Components
{
    public class RecentPostsSidebarViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public RecentPostsSidebarViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count = 5)
        {
            var items = await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();

            return View(items);
        }
    }
}