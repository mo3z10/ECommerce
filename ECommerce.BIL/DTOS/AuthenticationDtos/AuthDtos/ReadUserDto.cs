using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.AuthenticationDtos.AuthDtos
{
    public class ReadUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public ICollection<string> Roles { get; set; }

    }
}
