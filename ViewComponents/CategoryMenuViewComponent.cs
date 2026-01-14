using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetNews.Data;

namespace NetNews.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var categories = await _context.Categories
                                           .Include(c => c.SubCategories)
                                           .Where(c => c.ParentId == null && c.IsActive)
                                           .OrderBy(c => c.Name)
                                           .ToListAsync();

            return View(categories);
        }
    }
}