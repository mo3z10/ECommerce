using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ECommerce.DAL.Models
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public  string Description { get; set; }

        public double Price { get; set; }
        public  int QuntityInStock { get; set; }
        public string? ImageUrl { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public bool InStock { get; set; } = true;
        public byte[] RowVersion { get; set; }

    }
}
