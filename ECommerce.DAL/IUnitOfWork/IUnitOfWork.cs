using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.CartItemsRepo;
using ECommerce.DAL.Reposatories.CartRepo;
using ECommerce.DAL.Reposatories.CustomerRepo;
using ECommerce.DAL.Reposatories.GenericRepo;
using ECommerce.DAL.Reposatories.OrdersRepo;
using ECommerce.DAL.Reposatories.ProductRepo;

namespace ECommerce.DAL.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICartItemstRepo CarItemstRepo { get; }
        IProductRepo ProductsRepo { get; }
        IOrderRepo OrdersRepo { get; }
        ICartRepo  CartsRepo { get; }
        ICustomerRepo CustomersRepo { get; }
         Task<int> SaveChangesAsync();
        Task BeginTransaction();
        Task Rollback();
        Task Commit();

    }
}
