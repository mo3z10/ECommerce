using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;

namespace ECommerce.BIL.DTOS.CustomerDtos
{
    public class CustomerReadDto
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Address { get; set; } 
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
