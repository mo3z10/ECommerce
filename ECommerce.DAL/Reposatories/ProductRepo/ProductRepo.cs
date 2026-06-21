using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.GenericRepo;
using ECommerce.DAL.Reposatories.OrdersRepo;
using Microsoft.AspNetCore.Http;

namespace ECommerce.DAL.Reposatories.ProductRepo
{
    public class ProductRepo : GenericRepo<Product>,IProductRepo
    {
        private readonly ECommerceContext _context;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public ProductRepo(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor): base(eCommerceContext,httpContextAccessor) {
           _httpcontextAccessor = httpContextAccessor;
            _context = eCommerceContext;
        }
    }
}
