using System.Security.Claims;
using ECommerce.BIL.DTOS.CartDtos;
using ECommerce.BIL.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService; 
            
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(){
        
        return Ok( await _cartService.GetAllCartAsync());
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        [Authorize(Roles ="Admin")]

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult> GetByCustomerId(int customerId)
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(customerId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [Authorize]
        [HttpPost("items")]
        public async Task<ActionResult> AddItem(AddToCartDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _cartService.AddItemToCart(userId!, dto);

            return Ok(new
            {
                Message = "Item added successfully"
            });
        }

        [Authorize]
        [HttpDelete("items")]
        public async Task<ActionResult> RemoveItem(CartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _cartService.RemoveFromCart(userId!, dto);

            return NoContent();
        }
        [Authorize]
        [HttpPut("items/quantity")]
        public async Task<ActionResult> UpdateQuantity(UpdateCartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _cartService.UpdateQuantity(userId!, dto);

            return NoContent();
        }
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _cartService.ClearCart(userId!);

            return NoContent();
        }
        [Authorize]
[HttpGet("me")]
public async Task<ActionResult> GetMyCart()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var cart = await _cartService.GetMyCartAsync(userId!);

    return Ok(cart);
}


    }
}
