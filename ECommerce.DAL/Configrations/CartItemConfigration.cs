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
    public class CartItemConfigration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(a => a.Cart)
            .WithMany(a => a.cartItems)
            .HasForeignKey(c => c.CartId);
            builder.HasOne(a => a.Product)
            .WithMany(C => C.CartItems)
            .HasForeignKey(p => p.ProductId);
           
        }
    }
}
