using AllUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp.Controllers;

public class ChatController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public ChatController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Users = await _userManager.Users.ToListAsync();
        return View();
    }

    public async Task<IActionResult> Private(string id)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        if (User?.Identity?.Name == user.UserName) return RedirectToAction(nameof(Index));
        return View(user);
    }
}
