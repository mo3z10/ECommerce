using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.Services.JobSercvices
{
    public interface IJobService
    {
        void ApplyConfirmationOrderEmail(string email,int OrderId);
        void ApplyWelcomeEmail(string email);
        void ApplyOrderStatusEmail(string email, int orderId, OrderStatus status);
        void CleanupCarts();
        void LowStockMail();
    }
}
