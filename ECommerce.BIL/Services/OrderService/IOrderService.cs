using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.OrderDtos;
using ECommerce.DAL.PaginationFilterDtos;

namespace ECommerce.BIL.Services.OrderService
{
    public interface IOrderService
    {
        Task<PagedResult<OrderReadDto>> GetAllOrdersAsync(OrderFilterDto orderFilterDto);
        Task<PagedResult<OrderReadDto>> GetCustomerOrderAsync(string CustomerId,OrderFilterDto orderFilterDto);
        Task<OrderReadDto> GetOrderByIdAsync(int OrderId);
        Task DeleteOrderAsync(int OrderId);
        Task CreateOrderAsync(OrderAddDto OrderAddDto);
        Task UpdateOrderStatusAsync(OrderUpdateStatusDto orderUpdateStatusDto);
        Task Checkout(string CustomerId);

    }
}
