using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.PaginationFilterDtos
{
    public class PagedResult<T> 
    {
        public ICollection<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get
            {
                if(PageSize<0) return 0;
              return  (int)Math.Ceiling((double)TotalCount / PageSize);
} }
    }
}
