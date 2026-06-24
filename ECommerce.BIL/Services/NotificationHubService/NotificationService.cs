using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.SignalR;
using ECommerce.Api.HubService;


namespace ECommerce.BIL.Services.NotificationHubService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _Hub;
        public Task SendOrderUpdateStatus(string userId, int orderId, OrderStatus status)
        {
        }
    }
}
