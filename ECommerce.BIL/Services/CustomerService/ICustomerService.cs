using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.CustomerDtos;
using ECommerce.DAL.PaginationFilterDtos;

namespace ECommerce.BIL.Services.CustomerService
{
    public interface ICustomerService
    {
        Task<PagedResult<CustomerReadDto>> GetAllCustomersAsync(CustomerFilterDto filterDto);
        Task<CustomerReadDto?> GetCustomerByIdAsync(int customerId);

        Task<CustomerReadDto?> GetCustomerByUserIdAsync(string userId);
        Task UpdateCustomer(UpdateCustomerDto updateCustomerDto);
    }
}
