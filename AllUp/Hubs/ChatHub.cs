using Microsoft.AspNetCore.SignalR;

namespace AllUp.Hubs;

public class ChatHub : Hub
{
    public async Task SendAll(string message)
    {
        await Clients.All.SendAsync(message);
    }
    public async Task Send(string userId, string message)
    {
        await Clients.Client(userId).SendAsync(message);
    }
    public async Task Send(IEnumerable<string> userIds, string message)
    {
        await Clients.Clients(userIds).SendAsync(message);
    }
}
