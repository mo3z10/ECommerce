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

namespace ECommerce.DAL.Reposatories.CustomerRepo
{
    public class CustomerRepo : GenericRepo<Customer>,ICustomerRepo
    {
        private readonly ECommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerRepo(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor): base(eCommerceContext,httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
           _context = eCommerceContext;
        }
        public async Task<Customer?> GetByUserIdAsync(string userId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
