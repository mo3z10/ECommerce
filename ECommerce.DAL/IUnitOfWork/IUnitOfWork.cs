using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.CustomerRepo;
using ECommerce.DAL.Reposatories.GenericRepo;

namespace ECommerce.DAL.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<Product> ProductsRepo { get; }
        IGenericRepo<Order>OrdersRepo { get; }
        IGenericRepo <Cart> CartsRepo { get; }
        ICustomerRepo CustomersRepo { get; }
         Task<int> SaveChangesAsync();
        Task BeginTransaction();
        Task Rollback();
        Task Commit();

    }
}
