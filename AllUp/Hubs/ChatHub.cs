using AllUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace AllUp.Hubs;

public class ChatHub : Hub
{
    private readonly UserManager<AppUser> _userManager;

    public ChatHub(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SendAll(string message, string username)
    {
        await Clients.All.SendAsync("newMessage", message, username);
    }
    public async Task Send(string userId, string message)
    {
        string? connectionId = _userManager.FindByIdAsync(userId)?.Result?.ConnectionId;
        await Clients.Client(connectionId).SendAsync("newMessage", message, _userManager.FindByIdAsync(userId)?.Result?.UserName);
    }
    public override Task OnConnectedAsync()
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            AppUser? user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
            if (user != null)
            {
                user.ConnectionId = Context.ConnectionId;
                _userManager.UpdateAsync(user).Wait();
                Clients.All.SendAsync("userConnected", user.Id).Wait();
            }
        }
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            AppUser? user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
            if (user != null)
            {
                user.ConnectionId = null;
                _userManager.UpdateAsync(user).Wait();
                Clients.All.SendAsync("userDisconnected", user.Id).Wait();
            }
        }
        return base.OnDisconnectedAsync(exception);
    }
}
