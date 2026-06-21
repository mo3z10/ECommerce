using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.DTOS.CartDtos
{
    public class CartReadDto
    {
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }

        public ICollection<CartItemReadDto> Items { get; set; }
                  = new List<CartItemReadDto>();
    }
}
