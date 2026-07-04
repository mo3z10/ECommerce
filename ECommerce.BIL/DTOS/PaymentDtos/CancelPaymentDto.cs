using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace ECommerce.BIL.DTOS.PaymentDtos
{
    public class CancelPaymentDto
    {
       public string PaymentIntentId { get; set; }
    }
}
