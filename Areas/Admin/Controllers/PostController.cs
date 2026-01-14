using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetNews.Data;
using NetNews.Models;

namespace NetNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(posts);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            ModelState.Remove("AuthorId");

            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);
                post.AuthorId = currentUserId;

                if (string.IsNullOrEmpty(post.Slug))
                {
                    post.Slug = post.Title.ToLower().Replace(" ", "-");
                }

                if (post.ImageUpload != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/posts");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + post.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.ImageUpload.CopyToAsync(fileStream);
                    }
                    post.Thumbnail = "/uploads/posts/" + uniqueFileName;
                }

                post.CreatedAt = DateTime.Now;
                post.UpdatedAt = DateTime.Now;

                _context.Add(post);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đăng bài viết mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id) return NotFound();

            ModelState.Remove("AuthorId");
            ModelState.Remove("ImageUpload");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    if (existingPost == null) return NotFound();

                    post.AuthorId = existingPost.AuthorId;
                    post.CreatedAt = existingPost.CreatedAt;
                    post.UpdatedAt = DateTime.Now;

                    if (string.IsNullOrEmpty(post.Slug))
                    {
                        post.Slug = post.Title.ToLower().Replace(" ", "-");
                    }

                    if (post.ImageUpload != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/posts");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + post.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await post.ImageUpload.CopyToAsync(fileStream);
                        }
                        post.Thumbnail = "/uploads/posts/" + uniqueFileName;
                    }
                    else
                    {
                        post.Thumbnail = existingPost.Thumbnail;
                    }

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật bài viết thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Posts.Any(e => e.Id == post.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            return View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa bài viết thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}