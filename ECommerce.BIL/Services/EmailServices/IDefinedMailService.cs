using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.Services.EmailServices
{
    public interface IDefinedMailService
    {
        Task SendWelcomeEmail(string email);

        Task SendOrderConfirmationEmail(string email,int orderId);
        Task OrderStatusChangedEmail(string email, int orderId,OrderStatus status);
        Task LowStockMailService(string email, string Messege);
    }
}