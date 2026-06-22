using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.CartItemsRepo;
using ECommerce.DAL.Reposatories.CartRepo;
using ECommerce.DAL.Reposatories.CustomerRepo;
using ECommerce.DAL.Reposatories.GenericRepo;
using ECommerce.DAL.Reposatories.OrdersRepo;
using ECommerce.DAL.Reposatories.ProductRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DAL.IUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
       public ICartRepo CartsRepo { get; }
       public ICustomerRepo  CustomersRepo { get; }

        public ICartItemstRepo CarItemstRepo { get; }

        public IProductRepo ProductsRepo {  get; }

        public IOrderRepo OrdersRepo { get; }

        private readonly ECommerceContext _context;
        private IDbContextTransaction? _Transaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(ECommerceContext eCommerceContext,IHttpContextAccessor httpContextAccessor)
        {
            CarItemstRepo = new CartItemstRepo(eCommerceContext, httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
            _context = eCommerceContext;
            ProductsRepo = new ProductRepo(eCommerceContext,httpContextAccessor);
            OrdersRepo = new OrderRepo(eCommerceContext, httpContextAccessor);
            CartsRepo = new CartRepo(eCommerceContext, httpContextAccessor);
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
            var userId = _httpContextAccessor.HttpContext?.User?
           .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var entries = _context.ChangeTracker
    .Entries<BaseEntity>();

            foreach (var entry in entries)
            {

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = userId;
                }


                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = userId;
                }


                if (entry.State == EntityState.Deleted)
                {

                    if (entry.Entity is Product || entry.Entity is Order)
                    {
                        entry.State = EntityState.Modified;

                        entry.Entity.IsDeleted = true;
                    }
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.DeletedBy = userId;

                }

            }

            var products =_context.ChangeTracker.Entries<Product>()
        .Where(x => x.State == EntityState.Modified)
        .Select(x => x.Entity);


            foreach (var product in products)
            {
                product.InStock = product.QuntityInStock > 0;
            }

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