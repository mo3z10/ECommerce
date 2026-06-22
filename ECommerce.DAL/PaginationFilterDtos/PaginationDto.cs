using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.PaginationFilterDtos
{
    public abstract class PaginationDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
