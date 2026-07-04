using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.PaymentDtos
{
    public class CreatePaymentDto
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
