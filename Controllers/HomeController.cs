using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetNews.Data;
using NetNews.Models;
using System.Diagnostics;

namespace NetNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedPosts = await _context.Posts
                    .Include(p => p.Category)
                    .Include(p => p.Author)
                    .Where(p => p.IsPublished && p.IsFeatured)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3)
                    .ToListAsync(),

                RecentPosts = await _context.Posts
                    .Include(p => p.Category)
                    .Include(p => p.Author)
                    .Where(p => p.IsPublished && !p.IsFeatured)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(10)
                    .ToListAsync(),

                MostViewedPosts = await _context.Posts
                    .Where(p => p.IsPublished)
                    .OrderByDescending(p => p.ViewCount)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Category/{slug}")]
        public async Task<IActionResult> Category(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return RedirectToAction("Index");

            var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

            if (category == null) return NotFound();

            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => p.IsPublished && (p.CategoryId == category.Id || p.Category.ParentId == category.Id))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = category.Slug;

            return View(posts);
        }

        [Route("Post/Details/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return RedirectToAction("Index");

            var post = await _context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Category)
                        .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

            if (post == null) return NotFound();

            post.ViewCount++;
            _context.Update(post);
            await _context.SaveChangesAsync();

            ViewBag.RelatedPosts = await _context.Posts
                .Where(p => p.CategoryId == post.CategoryId && p.Id != post.Id && p.IsPublished)
                .OrderByDescending(p => p.CreatedAt)
                .Take(3)
                .ToListAsync();

            return View(post);
        }
    }
}