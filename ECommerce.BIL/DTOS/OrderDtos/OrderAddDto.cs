using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.OrderDtos
{
    public class OrderAddDto
    {
        public int CustomerId { get; set; }
        public ICollection<CreateOrderItemDto> Items { get; set; }

    }
}
