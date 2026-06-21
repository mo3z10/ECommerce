using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
