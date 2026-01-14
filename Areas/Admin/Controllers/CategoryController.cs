using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetNews.Data;
using NetNews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allCategories = await _context.Categories
                                                  .Include(c => c.Parent)
                                                  .ToListAsync();
            var sortedCategories = new List<Category>();

            void BuildTree(int? parentId)
            {
                var nodes = allCategories.Where(c => c.ParentId == parentId).ToList();

                foreach (var node in nodes)
                {
                    sortedCategories.Add(node);
                    BuildTree(node.Id);
                }
            }

            BuildTree(null);

            return View(sortedCategories);
        }

        public IActionResult Create()
        {
            ViewBag.ParentList = new SelectList(_context.Categories.Where(c => c.ParentId == null), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(category.Slug))
                {
                    category.Slug = category.Name.ToLower().Replace(" ", "-");
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tạo danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentList = new SelectList(_context.Categories.Where(c => c.ParentId == null), "Id", "Name", category.ParentId);
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var parents = _context.Categories.Where(c => c.Id != id && c.ParentId == null);
            ViewBag.ParentList = new SelectList(parents, "Id", "Name", category.ParentId);

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(category.Slug))
                    {
                        category.Slug = category.Name.ToLower().Replace(" ", "-");
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật danh mục thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(e => e.Id == category.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var parents = _context.Categories.Where(c => c.Id != id && c.ParentId == null);
            ViewBag.ParentList = new SelectList(parents, "Id", "Name", category.ParentId);
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                if (category.SubCategories != null && category.SubCategories.Any())
                {
                    _context.Categories.RemoveRange(category.SubCategories);
                }

                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa danh mục và các mục con liên quan!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}