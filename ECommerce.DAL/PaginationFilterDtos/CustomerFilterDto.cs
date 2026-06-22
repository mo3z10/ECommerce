using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.PaginationFilterDtos
{
    public class CustomerFilterDto : PaginationDto
    {
        public string SearchName { get; set; } = "";
        public string SearchEmail { get; set; } = "";
        public string SearchPhone { get; set; } = "";
        public string SearchAddress { get; set; } = "";
    }
}
