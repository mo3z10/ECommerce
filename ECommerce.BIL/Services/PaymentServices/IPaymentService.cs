using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.PaymentDtos;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BIL.Services.PaymentServices
{
    public interface IPaymentService
    {
         Task<PaymentResponseDto> CreatePaymentIntentAsync(CreatePaymentDto createPaymentDto);
         Task<string> GetPaymentStatus(string PaymentIntentId);
         Task CancelPayment(CancelPaymentDto cancelPaymentDto);
        Task<bool> VerifyWebhookAsync(HttpRequest request);
    }
}
