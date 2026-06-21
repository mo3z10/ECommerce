using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.AuthenticationDtos.EmailServicesDtos
{
    public  class ChangePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
