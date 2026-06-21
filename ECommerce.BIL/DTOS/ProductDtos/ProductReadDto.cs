using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.ProductDtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public bool InStock { get; set; } = true; 
        public string? ImgUrl { get; set; }
        public int? QuantityInStock { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
