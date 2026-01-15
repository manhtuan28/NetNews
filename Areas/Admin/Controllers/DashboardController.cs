using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetNews.Data;

namespace NetNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }

            ViewBag.PostCount = await _context.Posts.CountAsync();
            ViewBag.CategoryCount = await _context.Categories.CountAsync();
            ViewBag.UserCount = await _userManager.Users.CountAsync();

            ViewBag.TotalViews = await _context.Posts.SumAsync(p => p.ViewCount);

            var recentPosts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(recentPosts);
        }
    }
}