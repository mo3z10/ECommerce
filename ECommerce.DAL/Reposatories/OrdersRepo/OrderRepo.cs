using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.PaginationFilterDtos;
using ECommerce.DAL.Reposatories.GenericRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Reposatories.OrdersRepo
{
    public class OrderRepo :GenericRepo<Order> , IOrderRepo
    {
        private readonly ECommerceContext _context;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public OrderRepo(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor): base(eCommerceContext,httpContextAccessor) {
         
            _httpcontextAccessor = httpContextAccessor;
            _context = eCommerceContext;
            
        }

        public async Task<PagedResult<Order>> GetPagedAllAsync(OrderFilterDto filter)
        {
            var OrdersQuery = _context.Order.AsQueryable();
            if (!string.IsNullOrEmpty(filter.SearchCustomer))
            {
                OrdersQuery = OrdersQuery.Where(o => o.Customer.UserName.Contains(filter.SearchCustomer));
            }
            if (filter.minTotalPrice.HasValue)
            {
                OrdersQuery = OrdersQuery.Where(o => o.TotalPrice >= filter.minTotalPrice);
            }
            if (filter.maxTotalPrice.HasValue)
            {
                OrdersQuery = OrdersQuery.Where(o => o.TotalPrice <= filter.maxTotalPrice);
            }
            if (filter.MaxQuaintiy.HasValue)
            {
                OrdersQuery = OrdersQuery.Where(o => o.TotalQuintiy<= filter.MaxQuaintiy);
            }
            if (filter.MinQuaintiy.HasValue)
            {
                OrdersQuery = OrdersQuery.Where(o => o.TotalQuintiy >= filter.MinQuaintiy);
            }
            if (!string.IsNullOrEmpty(filter.orderStatus))
            {
                OrdersQuery = OrdersQuery.Where(o => o.OrderStatus.ToString() == filter.orderStatus);

            }
            var FilteredOrders = await OrdersQuery.ToListAsync();
            return new PagedResult<Order>
            {
                Items = FilteredOrders,
                TotalCount = FilteredOrders.Count,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
            };
        }
    }
}
