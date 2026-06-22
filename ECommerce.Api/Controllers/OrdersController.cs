using System.Security.Claims;
using ECommerce.BIL.DTOS.OrderDtos;
using ECommerce.BIL.Services.OrderService;
using ECommerce.DAL.PaginationFilterDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.Checkout(userId);

            return Ok(new
            {
                Message = "Order created successfully"
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery]OrderFilterDto orderFilterDto)
        {
            var orders = await _orderService.GetAllOrdersAsync(orderFilterDto);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            return Ok(order);
        }

        [HttpGet("my-orders")]
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _orderService.GetCustomerOrderAsync(userId);

            return Ok(orders);
        }

        [HttpPatch("status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(
            OrderUpdateStatusDto dto)
        {
            await _orderService.UpdateOrderStatusAsync(dto);

            return Ok(new
            {
                Message = "Order status updated successfully"
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);

            return NoContent();
        }
    }
}