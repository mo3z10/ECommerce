using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.Services.EmailServices;
using ECommerce.DAL.IUnitOfWork;
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Core;

namespace ECommerce.BIL.Services.InventoryJob
{
    public class InventoryJob : IInventoryJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDefinedMailService _emailService;
        private readonly IConfiguration _configuration;
        public InventoryJob(IUnitOfWork unitOfWork,IDefinedMailService emailService,IConfiguration configuration)
        {
            _emailService= emailService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task CheckLowStock()
        {
            var LowProduct = await _unitOfWork.ProductsRepo.GetLowStockProductsAsync(5);
            if (!LowProduct.Any())
            {
                return;
            }
            var message = string.Join(Environment.NewLine, LowProduct.Select(x => $"{x.Name}:{x.QuntityInStock}"));
            await _emailService.LowStockMailService(_configuration["Admin:AdminMail"],message);
        }
    }
}
