using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerce.DAL.Database
{
    public class ECommerceContext : IdentityDbContext<ApplicationUser>
    {
        private static LambdaExpression IsDeleted(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var Property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var Equal = Expression.Equal(Property, Expression.Constant(false));
            return Expression.Lambda(Equal, parameter);

        }
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ECommerceContext).Assembly);
            base.OnModelCreating(builder);
            foreach (var type in builder.Model.GetEntityTypes()) {
                if (typeof(BaseEntity).IsAssignableFrom(type.ClrType) && type.BaseType == null)
                {
                    builder.Entity(type.ClrType).HasQueryFilter(IsDeleted(type.ClrType));
                }
            
            }

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
