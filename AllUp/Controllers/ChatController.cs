using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AllUp.Controllers;

public class ChatController : Controller
{
    private readonly IHubContext _hubContext;

    public ChatController(IHubContext hubContext)
    {
        _hubContext = hubContext;
    }

    public IActionResult Index()
    {
        return View();
    }
}
