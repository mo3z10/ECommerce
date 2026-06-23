using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.Services.CartJob;
using ECommerce.BIL.Services.EmailServices;
using ECommerce.BIL.Services.InventoryJob;
using ECommerce.DAL.Models;
using Hangfire;

namespace ECommerce.BIL.Services.JobSercvices
{
    public class HangfireJopServic : IJobService
    {
        private readonly IBackgroundJobClient _BackGroundJob;
        public HangfireJopServic(IBackgroundJobClient backgroundJobClient)
        {
            _BackGroundJob = backgroundJobClient;
            
        }
        public void ApplyConfirmationOrderEmail(string email, int OrderId)
        {
            _BackGroundJob.Enqueue<IDefinedMailService>(x => x.SendOrderConfirmationEmail(email, OrderId));
        }

        public void ApplyOrderStatusEmail(string email, int orderId, OrderStatus status)
        {
            _BackGroundJob.Enqueue<IDefinedMailService>(x => x.OrderStatusChangedEmail(email, orderId, status));
        }

        public void ApplyWelcomeEmail(string email)
        {
            _BackGroundJob.Enqueue<IDefinedMailService>(x => x.SendWelcomeEmail(email));
        }

        public void CleanupCarts()
        {
            _BackGroundJob.Enqueue<ICartJob>(x => x.RemoveAbandonedCarts());
        }

        public void LowStockMail()
        {
            _BackGroundJob.Enqueue<IInventoryJob>(x => x.CheckLowStock());
        }
    }
}
