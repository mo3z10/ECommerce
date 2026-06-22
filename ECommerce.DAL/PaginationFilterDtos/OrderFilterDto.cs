using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.DAL.PaginationFilterDtos
{
    public class OrderFilterDto :PaginationDto
    {
        public string SearchCustomer { get; set; } = "";
        public double? minTotalPrice { get; set; } 
        public double? maxTotalPrice { get; set; } 
        public int? MaxQuaintiy {  get; set; }
        public int? MinQuaintiy {  get; set; }
        public string orderStatus { get; set; } = "";
        
    }
}
