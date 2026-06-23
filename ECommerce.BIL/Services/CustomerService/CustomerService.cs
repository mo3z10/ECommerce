using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.CustomerDtos;
using ECommerce.BIL.Services.CacheService;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.PaginationFilterDtos;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.BIL.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        public CustomerService(IUnitOfWork unitOfWork,ICacheService cache)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CustomerReadDto>> GetAllCustomersAsync(CustomerFilterDto filterDto)
        {
            var Version = await _cache.GetVersionAsync("customers");
            var key =
$"customers_{Version}_{filterDto.PageNumber}_{filterDto.PageSize}_{filterDto.SearchName}_{filterDto.SearchPhone}_{filterDto.SearchAddress}_{filterDto.SearchEmail}_{filterDto.Sortby}_{filterDto.IsDescending}";
            var Cached = await _cache.GetAsync <PagedResult<CustomerReadDto>>(key);
            if (Cached != null) {
                return Cached;
            }
            var Customers = await _unitOfWork.CustomersRepo.GetPagedAllAsync(filterDto);
            var ResultCustomers = new PagedResult<CustomerReadDto>
            {
                Items =  Customers.Items.Select(p => new CustomerReadDto
                {
                    Id = p.Id,
                    Address = p.Address,
                    PhoneNumber = p.PhoneNumber,
                    UserName = p.UserName,

                }).ToList(),
                TotalCount = Customers.TotalCount
                , PageNumber = Customers.PageNumber
                , PageSize = Customers.PageSize
            };
            await _cache.SetAsync(key, ResultCustomers, 5);
            return ResultCustomers;
        }

        public async Task<CustomerReadDto> GetCustomerByIdAsync(int id)
        {
            var key = $"Customer_{id}";
            var Cached = await _cache.GetAsync<CustomerReadDto>(key);
            if (Cached != null)
            {
                return Cached;
            }
            var Customer = await _unitOfWork.CustomersRepo.GetByIdAsync(id);
            if (Customer == null)
            {
                return null;
            }
           var CustomerModel = new CustomerReadDto()
           {
               Id = Customer.Id,
               Address = Customer.Address,
               PhoneNumber = Customer.PhoneNumber,
               UserName= Customer.UserName,
           };
            await _cache.SetAsync(key, CustomerModel, 5);
            return CustomerModel;

        }

        public async Task<CustomerReadDto?> GetCustomerByUserIdAsync(string userId)
        {

            var customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);
            if (customer == null)
            {
                return null;
            }
            var key = $"Customer_{customer.Id}";
            var Cached = await _cache.GetAsync<CustomerReadDto>(key);
            if (Cached != null)
            {
                return Cached;
            }

            var customerModel = new CustomerReadDto()
            {
                Id = customer.Id,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                UserName = customer.UserName,
            };
            await _cache.SetAsync(key, customerModel, 5);
            return customerModel;
        }

        public async Task UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _unitOfWork.CustomersRepo.GetByIdAsync(updateCustomerDto.Id);
            if (customer == null)
            {
                throw new KeyNotFoundException("CustomerNotFound");
             }
            customer.Address = updateCustomerDto.Address;
            customer.PhoneNumber = updateCustomerDto.PhoneNumber;
            await _cache.RefreshVersionAsync("customers");
            await _cache.RemoveAsync($"Customer_{updateCustomerDto.Id}");
            await _unitOfWork.SaveChangesAsync();
            }
       
    }
}
