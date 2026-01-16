using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy danh sách các danh mục chính có chứa bài viết
            var categories = await _context.Categories
                .Where(c => c.IsActive && c.ParentId == null)
                .ToListAsync();

            var categoryBoxes = new List<CategoryBoxViewModel>();

            foreach (var cat in categories)
            {
                var posts = await _context.Posts
                    .Include(p => p.Author)
                    .Where(p => p.IsPublished && (p.CategoryId == cat.Id || p.Category.ParentId == cat.Id))
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(4) // Mỗi danh mục lấy 4 bài
                    .ToListAsync();

                if (posts.Any())
                {
                    categoryBoxes.Add(new CategoryBoxViewModel
                    {
                        CategoryName = cat.Name,
                        CategorySlug = cat.Slug,
                        Posts = posts
                    });
                }
            }

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
                    .Where(p => p.IsPublished)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(6)
                    .ToListAsync(),

                MostViewedPosts = await _context.Posts
                    .Where(p => p.IsPublished)
                    .OrderByDescending(p => p.ViewCount)
                    .Take(5)
                    .ToListAsync(),

                CategoryBoxes = categoryBoxes
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Home/PageNotFound")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostComment(int postId, string content, int rating, int? parentId)
        {
            if (string.IsNullOrWhiteSpace(content)) return BadRequest();

            var comment = new Comment
            {
                PostId = postId,
                Content = content,
                Rating = rating,
                ParentId = parentId,
                UserId = _userManager.GetUserId(User),
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var post = await _context.Posts.FindAsync(postId);
            return RedirectToAction("Details", new { slug = post.Slug });
        }

        [Route("Post/Details/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return RedirectToAction("Index");

            var post = await _context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Category)
                        .Include(p => p.Comments).ThenInclude(c => c.User)
                        .Include(p => p.Comments).ThenInclude(c => c.Replies).ThenInclude(r => r.User)
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