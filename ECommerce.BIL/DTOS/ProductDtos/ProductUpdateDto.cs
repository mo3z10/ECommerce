using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ECommerce.BIL.DTOS.ProductDtos
{
    public class ProductUpdateDto

    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }

        public string? ImagUrl { get; set; }
        public bool InStock { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public byte[] RowVersion { get; set; }




    }
}
