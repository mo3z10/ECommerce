using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.CustomerDtos;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.PaginationFilterDtos;

namespace ECommerce.BIL.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CustomerReadDto>> GetAllCustomersAsync(CustomerFilterDto filterDto)
        {
            var Customers = await _unitOfWork.CustomersRepo.GetPagedAllAsync(filterDto);
            return new PagedResult<CustomerReadDto>
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
        }

        public async Task<CustomerReadDto> GetCustomerByIdAsync(int id)
        {
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
            return CustomerModel;

        }

        public async Task<CustomerReadDto?> GetCustomerByUserIdAsync(string userId)
        {
            var customer = await _unitOfWork.CustomersRepo.GetByUserIdAsync(userId);
            if (customer == null)
            {
                return null;
            }
            var customerModel = new CustomerReadDto()
            {
                Id = customer.Id,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                UserName = customer.UserName,
            };
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
            await _unitOfWork.SaveChangesAsync();
            }
       
    }
}
