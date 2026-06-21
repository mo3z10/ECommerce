using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.OrderDtos;

namespace ECommerce.BIL.Services.OrderService
{
    public class OrderService : IOrderService
    {
        public Task Checkout(string CustomerId)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrderAsync(OrderAddDto OrderAddDto)
        {

        }

        public Task DeleteOrderAsync(int OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderReadDto>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderReadDto>> GetCustomerOrderAsync(string CustomerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderStatusAsync(OrderUpdateStatusDto orderUpdateStatusDto)
        {
            throw new NotImplementedException();
        }
    }
}
