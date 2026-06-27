using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Shared.HubService
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin")){
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }
           Console.WriteLine($"Connected: {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }
    }
}
