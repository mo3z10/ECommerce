using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.IUnitOfWork;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECommerce.BIL.Services.CartJob
{
    public class CartJob : ICartJob
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task RemoveAbandonedCarts()
        {
            var Carts = await _unitOfWork.CartsRepo.GetAbandonedCartsAsync(DateTime.UtcNow.AddHours(-24));
            foreach (var cart in Carts) {
                await _unitOfWork.CartsRepo.ClearCartAsync(cart.Id);
            }

        }
    }
}
