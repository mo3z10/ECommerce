using System.Security.Claims;
using ECommerce.BIL.DTOS.CustomerDtos;
using ECommerce.BIL.Services.CustomerService;
using ECommerce.DAL.PaginationFilterDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> GetCurrentCustomer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest();

            }

            var customer = await _customerService.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync([FromQuery] CustomerFilterDto customerFilterDto)
        {
            var customers = await _customerService.GetAllCustomersAsync(customerFilterDto);
            if (customers == null)
            {
             return NotFound();
            }
            return Ok(customers);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var Customer = await _customerService.GetCustomerByIdAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }
            return Ok(Customer);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto,int id)
        {
            if (updateCustomerDto.Id != id)
            {
                return BadRequest();
            }
            await _customerService.UpdateCustomer(updateCustomerDto);

            return NoContent();
        }
    }
}
