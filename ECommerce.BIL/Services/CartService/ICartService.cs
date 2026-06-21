using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.CartDtos;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.Services.CartService
{
    public interface ICartService
    {
        Task<CartReadDto?> GetCartByIdAsync(int Id);
        Task<CartReadDto?> GetCartByCustomerIdAsync(int Id);
        Task<ICollection<CartReadDto>> GetAllCartAsync();
        Task AddItemToCart(string userId, AddToCartDto dto);

        Task RemoveFromCart(string userId, CartItemDto dto);

        Task UpdateQuantity(string userId, UpdateCartItemDto dto);

        Task ClearCart(string userId);
        Task<CartReadDto?> GetMyCartAsync(string userId);
    }
}
