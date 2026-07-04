using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.PaymentDtos
{
    public class PaymentResponseDto
    {
        public string PaymentIntentId { get; set; }
        public string Secret { get; set; }
        public string Status { get; set; }

    }
}
