using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.DTOS.OrderDtos
{
    public class OrderUpdateStatusDto
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
