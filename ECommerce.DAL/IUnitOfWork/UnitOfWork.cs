using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.CustomerRepo;
using ECommerce.DAL.Reposatories.GenericRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DAL.IUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
       public IGenericRepo<Product> ProductsRepo { get; }
       public IGenericRepo<Order> OrdersRepo { get; }
       public IGenericRepo<Cart> CartsRepo { get; }
       public ICustomerRepo  CustomersRepo { get; }

        private readonly ECommerceContext _context;
        private IDbContextTransaction _Transaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor)
        { 
            _httpContextAccessor = httpContextAccessor;
            _context = eCommerceContext;
            ProductsRepo = new GenericRepo<Product>(eCommerceContext,httpContextAccessor);
            OrdersRepo = new GenericRepo<Order>(eCommerceContext, httpContextAccessor);
            CartsRepo = new GenericRepo<Cart>(eCommerceContext, httpContextAccessor);
            CustomersRepo = new CustomerRepo(eCommerceContext, httpContextAccessor);
        }
        public async Task BeginTransaction()
        {
            _Transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_Transaction != null)
                {
                    await _Transaction.CommitAsync();
                }
            }
            catch
            {
                await Rollback();
                throw;
            }
        }

        public void Dispose()
        {
           _context.Dispose();
            _Transaction?.Dispose();
        }

        public async Task Rollback()
        {
            if (_Transaction != null)
            {
                await _Transaction.RollbackAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    await entry.ReloadAsync();
                }

                throw new DbUpdateConcurrencyException(
                    "This record has been modified by another user. Please refresh and try again.");
            }
        }
    }
}