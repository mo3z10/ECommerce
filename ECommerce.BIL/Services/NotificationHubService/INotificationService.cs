using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.Services.NotificationHubService
{
    public interface INotificationService
    {
        Task SendOrderUpdateStatus(string userId, int orderId, OrderStatus status);
        Task SendLowStockNotification(string Messege);
        Task NewOrderCreated(int OrderId);
        Task NewCustomerRegistered(string CustomerName);

    }
}
