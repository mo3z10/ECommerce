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
    public class CartConfigration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasOne(c => c.Customer)
             .WithOne(c => c.cart)
             .HasForeignKey<Cart>(c => c.CustomerID)
             .IsRequired()
             .OnDelete(deleteBehavior:DeleteBehavior.Cascade);

        }
    }
}
