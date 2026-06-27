using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.SignalR;
using ECommerce.Shared.HubService;


namespace ECommerce.BIL.Services.NotificationHubService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _Hub;
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _Hub = hubContext;
            
        }

        public async Task NewCustomerRegistered(string CustomerName)
        {
            await _Hub.Clients.Group("Admin").SendAsync("NewCustomerRegesteredAlert", CustomerName);
        }

        public async Task NewOrderCreated(int OrderId)
        {
            Console.WriteLine("Test If Work");
            await _Hub.Clients.Group("Admin").SendAsync("NewOrderCreated", OrderId);
        }

        public async Task SendLowStockNotification(string Messege)
        {
            await _Hub.Clients.Group("Admin").SendAsync("LowStockAlert",Messege);
        }


        public async Task SendOrderUpdateStatus(string userId, int orderId, OrderStatus status)
        {
            await _Hub.Clients.User(userId).SendAsync("OrderUpdated",orderId, status.ToString());
        }
    }
}
