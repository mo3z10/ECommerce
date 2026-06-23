using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ECommerce.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ResetCode { get; set; }

        public DateTime? ResetCodeExpire { get; set; }

        public virtual Customer? Customer { get; set; }


    }
}
