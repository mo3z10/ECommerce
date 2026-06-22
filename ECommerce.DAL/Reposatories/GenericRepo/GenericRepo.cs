using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Database;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Reposatories.GenericRepo
{
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ECommerceContext _context;

        public GenericRepo(ECommerceContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var Query =   _context.Set<TEntity>().AsQueryable();
            bool isAdmin = _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
            if (isAdmin)
            {
                Query = Query.IgnoreQueryFilters();
            }
            return await Query.FirstOrDefaultAsync((e =>
            EF.Property<int>(e, "Id") == id));

        }


        public Task UpdateAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

     public async Task<IQueryable<TEntity>>GetAllAsync()
        {
           var query = _context.Set<TEntity>().AsQueryable();
            bool IsAdmin = _httpContextAccessor.HttpContext?.User .IsInRole("Admin") ?? false; 
            if (IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
            return query;
        }
        public async Task SetRowVersion(Product product, byte[] rowVersion)
        {
            _context.Entry(product)
                .Property(p => p.RowVersion)
                .OriginalValue = rowVersion;
        }
    }
}
