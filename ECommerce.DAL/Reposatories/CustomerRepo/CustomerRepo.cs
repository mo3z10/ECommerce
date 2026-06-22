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
using Microsoft.IdentityModel.Protocols;

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

        public async Task<PagedResult<Customer>> GetPagedAllAsync(CustomerFilterDto filterDto)
        {
            var CustomersQuery = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(filterDto.SearchName))
            {
                CustomersQuery = CustomersQuery.Where(p => p.UserName.Contains(filterDto.SearchName));

            }
            if (!string.IsNullOrEmpty(filterDto.SearchAddress))
            {
                CustomersQuery = CustomersQuery.Where(p => p.Address.Contains(filterDto.SearchAddress));

            }
            if (!string.IsNullOrEmpty(filterDto.SearchPhone))
            {
                CustomersQuery = CustomersQuery.Where(p => p.PhoneNumber.Contains(filterDto.SearchPhone));

            }
            if (!string.IsNullOrEmpty(filterDto.SearchEmail))
            {
                CustomersQuery = CustomersQuery.Where(p => p.ApplicationUser.Email.Contains(filterDto.SearchEmail));

            }
            CustomersQuery = ApplySort(CustomersQuery, filterDto);
            var TotalCount  = await CustomersQuery.CountAsync();
            var PagedCustomer = await CustomersQuery.Skip((filterDto.PageNumber -1 ) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ToListAsync();
            return new PagedResult<Customer>
            {
                Items = PagedCustomer,
                PageNumber = filterDto.PageNumber,
                TotalCount = TotalCount
            };
        }

        public async Task<Customer?> GetByUserIdAsync(string userId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        private IQueryable<Customer> ApplySort(IQueryable<Customer> query, CustomerFilterDto filter)
        {
            switch (filter.Sortby?.ToLower())
            {
                case "name":
                    return
                    (filter.IsDescending) ?
                    query.OrderByDescending(x => x.UserName) :
                    query.OrderBy(p => p.UserName);
                case "email":
                    return
                    (filter.IsDescending) ?
                  query.OrderByDescending(x => x.ApplicationUser.Email) :
                  query.OrderBy(p => p.ApplicationUser.Email);

                case "phone":
                    return
                    (filter.IsDescending) ?
                    query.OrderByDescending(x => x.PhoneNumber) :
                    query.OrderBy(p => p.PhoneNumber);
                case "address":
                    return
                    (filter.IsDescending) ?
                    query.OrderByDescending(x => x.Address) :
                    query.OrderBy(p => p.Address);
                case "NumberOfOrder":
                    return
                    (filter.IsDescending) ?
                    query.OrderByDescending(x => x.orders.Count()):
                    query.OrderBy(p => p.orders.Count);
                default: return query.OrderBy(x => x.CreatedAt);
            }
        }
    }
}
