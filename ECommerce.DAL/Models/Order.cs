using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Models
{
    public class Order : BaseEntity
    {
        public  int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
       
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int TotalQuintiy { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
   public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled   
    }
}
