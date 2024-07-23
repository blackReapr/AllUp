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
}
