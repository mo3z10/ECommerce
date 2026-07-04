using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.PaymentDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace ECommerce.BIL.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        public PaymentService(IConfiguration configuration)
        {
           _configuration = configuration;
        }
        public async Task CancelPayment(CancelPaymentDto cancelPaymentDto)
        {
            var Service = new PaymentIntentService();
             await Service.CancelAsync(cancelPaymentDto.PaymentIntentId);
        }

        public async Task<PaymentResponseDto> CreatePaymentIntentAsync(CreatePaymentDto createPaymentDto)
        {
            var Options = new PaymentIntentCreateOptions
            {
                Amount = (long)(createPaymentDto.Amount * 100), 
                Currency = createPaymentDto.CurrencyCode,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new PaymentIntentService();
           var PaymentIntent =  await service.CreateAsync(Options);
            return new PaymentResponseDto
            {
                PaymentIntentId = PaymentIntent.Id,
                Secret = PaymentIntent.ClientSecret,
                Status = PaymentIntent.Status
            };
        }

        public async Task<string> GetPaymentStatus(string PaymentIntentId)
        {
            var Service = new PaymentIntentService();
            var PaymentIntent = await Service.GetAsync(PaymentIntentId);
            return PaymentIntent.Status;
        }

        public async Task<bool> VerifyWebhookAsync(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookSecret"]
                    
                );

                return stripeEvent.Type == "payment_intent.succeeded";
            }
            catch
            {
                return false;
            }
        }
    }
}
