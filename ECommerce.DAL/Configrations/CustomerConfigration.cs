using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DAL.Configrations
{
    public class CustomerConfigration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(a => a.ApplicationUser)
            .WithOne(a => a.Customer)
            .HasForeignKey<Customer>(a => a.UserId)
            .OnDelete(deleteBehavior:DeleteBehavior.Cascade);

            builder.HasOne(c => c.cart)
                .WithOne(c => c.Customer)
                .HasForeignKey<Cart>(c => c.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}