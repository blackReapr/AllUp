using AllUp.Data;
using AllUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp.Areas.Manage.Controllers;

[Area("Manage")]
public class ProductController : Controller
{
    private readonly AllUpDbContext _context;

    public ProductController(AllUpDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> products = await _context.Products.AsNoTracking().Include(p => p.Category).Include(p => p.Brand).Where(p => !p.IsDeleted).ToListAsync();
        return View(products);
    }

    public IActionResult Create()
    {
        return View();
    }
}
