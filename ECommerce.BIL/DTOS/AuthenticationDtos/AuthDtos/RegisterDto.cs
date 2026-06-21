using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.DTOS.AuthenticationDtos.AuthDtos
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }
    }
}
