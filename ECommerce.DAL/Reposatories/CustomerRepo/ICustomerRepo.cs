using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.GenericRepo;

namespace ECommerce.DAL.Reposatories.CustomerRepo
{
    public interface ICustomerRepo:IGenericRepo<Customer>
    {
        Task<Customer?> GetByUserIdAsync(string userId);
    }
}
