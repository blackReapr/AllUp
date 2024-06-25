using AllUp.Data;
using AllUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AllUpDbContext _context;

        public ProductController(AllUpDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Modal(int? id)
        {
            if (id == null) return BadRequest();
            Product? product = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (product == null) return NotFound();
            return PartialView("_ProductModal", product);
        }
    }
}
