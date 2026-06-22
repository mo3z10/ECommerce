using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.PaginationFilterDtos
{
    public class ProductFilterDto :PaginationDto
    {
        public string? Search { get; set; }
        public int? MaxQuantity { get; set; }
        public int? MinQuantity { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public bool? InStock { get; set; } = true;
        public string? Sortby { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
