using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.GenericRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Reposatories.CartRepo
{
    public class CartRepo : GenericRepo<Cart> , ICartRepo
    {
        ECommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepo(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor):base(eCommerceContext,httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = eCommerceContext;
        }

        public async Task ClearCartAsync(int cartId)
        {
            await _context.CartItem
                .Where(x => x.CartId == cartId)
                .ExecuteDeleteAsync();
        }
        public async Task<List<Cart>> GetAbandonedCartsAsync(DateTime cutoffDate)
        {
            return await _context.Cart
                .Where(c => c.UpdatedAt < cutoffDate)
                .ToListAsync();
        }

    }
}
