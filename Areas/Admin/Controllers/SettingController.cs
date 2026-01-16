using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetNews.Data;
using NetNews.Models;
using Microsoft.EntityFrameworkCore;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SettingController : Controller
{
    private readonly ApplicationDbContext _context;

    public SettingController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var setting = await _context.SystemSettings.FirstOrDefaultAsync()
                      ?? new SystemSetting();
        return View(setting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SystemSetting model)
    {
        if (ModelState.IsValid)
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync();
            if (setting == null)
            {
                _context.Add(model);
            }
            else
            {
                setting.WebsiteName = model.WebsiteName;
                setting.Slogan = model.Slogan;
                setting.ContactEmail = model.ContactEmail;
                setting.PhoneNumber = model.PhoneNumber;
                setting.Address = model.Address;
                setting.FooterText = model.FooterText;
                setting.FacebookUrl = model.FacebookUrl;
                setting.YoutubeUrl = model.YoutubeUrl;
                _context.Update(setting);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã lưu cấu hình hệ thống!";
        }
        return RedirectToAction(nameof(Index));
    }
}