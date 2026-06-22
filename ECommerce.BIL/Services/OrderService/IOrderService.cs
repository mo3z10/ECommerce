using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.OrderDtos;

namespace ECommerce.BIL.Services.OrderService
{
    public interface IOrderService
    {
        Task<ICollection<OrderReadDto>> GetAllOrdersAsync();
        Task<ICollection<OrderReadDto>> GetCustomerOrderAsync(string CustomerId);
        Task<OrderReadDto> GetOrderByIdAsync(int OrderId);
        Task DeleteOrderAsync(int OrderId);
        Task CreateOrderAsync(OrderAddDto OrderAddDto);
        Task UpdateOrderStatusAsync(OrderUpdateStatusDto orderUpdateStatusDto);
        Task Checkout(string CustomerId);

    }
}
