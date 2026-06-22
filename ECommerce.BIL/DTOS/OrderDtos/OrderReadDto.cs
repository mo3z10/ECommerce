using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.DTOS.OrderDtos
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double totalPrice { get; set; }
        public int totalQuantity { get; set; }
        public String OrderStatus { get; set; }
        public ICollection<ReadOrderItemDto> Items {  get; set; }

    }
}
