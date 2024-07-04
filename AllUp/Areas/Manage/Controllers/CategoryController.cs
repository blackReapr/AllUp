using AllUp.Data;
using AllUp.Extensions;
using AllUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp.Areas.Manage.Controllers;

[Area("Manage")]
public class CategoryController : Controller
{
    private readonly AllUpDbContext _context;

    public CategoryController(AllUpDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> categories = await _context.Categories.Include(c => c.Products).Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
        return View(categories);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        if (category.IsMain)
        {
            if (category.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo is required");
                return View(category);
            }
            if (!category.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Invalid file type");
                return View(category);
            }
            if (category.Photo.DoesSizeExceed(150))
            {
                ModelState.AddModelError("Photo", "File size limit exceeded");
                return View(category);
            }
            category.Image = await category.Photo.SaveFileAsync();
            category.ParentId = null;
        }
        else
        {
            if (category.ParentId == null || !await _context.Categories.AnyAsync(c => !c.IsDeleted && c.IsMain))
            {
                ModelState.AddModelError("ParentId", "Invalid parent id");
                return View(category);
            }
            category.Photo = null;
        }
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
