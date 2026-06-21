using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Models
{ 
    //customer 
    //product 
    //cart
    //order
    // customer => cart
    // customer => m orders 
    // 
   
    public class Customer : BaseEntity
    {
        public string UserId { get; set; }
        public string Address {get; set; } 
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
     
        public virtual ICollection<Order>? orders { get; set; }
        public virtual  Cart cart { get; set; } 
      public virtual ApplicationUser ApplicationUser { get; set; }
       
    }
}
