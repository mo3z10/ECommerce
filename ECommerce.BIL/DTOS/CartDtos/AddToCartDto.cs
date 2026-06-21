using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.CartDtos
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quaintity { get; set; }
    }
}
