using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Models
{
    public class Cart : BaseEntity 
    {
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<CartItem> cartItems { get; set; } = new List<CartItem>();

    }
}
