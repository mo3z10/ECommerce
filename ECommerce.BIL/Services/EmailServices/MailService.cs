using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using NETCore.MailKit.Core;

namespace ECommerce.BIL.Services.EmailServices
{
    public class DefinedMailService : IDefinedMailService
    {
        private readonly IEmailService _MailKitService;
        public DefinedMailService(IEmailService mailService )
        {
            _MailKitService = mailService;
        }

        public async Task LowStockMailService(string email, string Messege)
        {
            await _MailKitService.SendAsync(email,"Low Stock Alert",Messege);
        }

        public async Task OrderStatusChangedEmail(string email, int orderId,OrderStatus status)
        {
            await _MailKitService.SendAsync(email,
            $"Your Order number {orderId} Staus Has Been Changed to {status.ToString()} "," Please Go to your Orders and Check It");
        }

        public async Task SendOrderConfirmationEmail(string email, int orderId)
        {
            await _MailKitService.SendAsync(email,
             "Your Order Has Been Confirmed", $"Order Id : {orderId} Please Go to your Orders and Check It");

        }

        public async Task SendWelcomeEmail(string email)
        {
            await _MailKitService.SendAsync(email, $"Welcome To Our ECommerce {email} ","Hope u enjou here");
        }
    }
}
