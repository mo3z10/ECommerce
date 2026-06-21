using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace ECommerce.DAL.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual Order Order { get; set; }
        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
