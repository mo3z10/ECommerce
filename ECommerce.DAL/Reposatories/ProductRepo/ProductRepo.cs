using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.PaginationFilterDtos;
using ECommerce.DAL.Reposatories.GenericRepo;
using ECommerce.DAL.Reposatories.OrdersRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagedResult<Product>> GetProductsAsync(ProductFilterDto filter)
        {
            var query = _context.Product.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(p => p.Name.Contains(filter.Search));
            }
            if (filter.InStock.HasValue)
            {
                query = query.Where(p =>
                    p.InStock == filter.InStock.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice);
            }
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >=filter.MinPrice);
            }
            
            if (filter.MinQuantity.HasValue)
            {
                query = query.Where(p => p.QuntityInStock >=filter.MinQuantity);
            }
            
            if (filter.MaxQuantity.HasValue)
            {
                query = query.Where(p => p.QuntityInStock <=filter.MaxQuantity);
            }
            
            var TotalCount = await query.CountAsync();
            var products = await query.Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .ToListAsync();
            return new PagedResult<Product>
            {
                Items = products,
                TotalCount = TotalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
            };
        }
    }
}
