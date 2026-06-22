using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.GenericRepo;
using Microsoft.AspNetCore.Http;

namespace ECommerce.DAL.Reposatories.CartItemsRepo
{
    public class CartItemstRepo :GenericRepo<CartItem>,ICartItemstRepo
    {
        ECommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartItemstRepo(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor): base(eCommerceContext, httpContextAccessor)
        {
            _context = eCommerceContext;
            _httpContextAccessor = httpContextAccessor;
            
        }
    }
}
