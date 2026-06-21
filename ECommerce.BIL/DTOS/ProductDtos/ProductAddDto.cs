using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.ProductDtos
{
    public class ProductAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]


        public int QuantityInStock { get; set; }
        public double Price { get; set; }
       
        public string? ImgUrl { get; set; }


    }
}
