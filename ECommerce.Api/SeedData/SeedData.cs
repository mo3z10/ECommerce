using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Api.SeedData
{
    public class SeedData
    {
        public static async Task SeedAdmin(UserManager<ApplicationUser> _UserManager, RoleManager<IdentityRole> _RoleManager)
        {
        
            if (!await _RoleManager.RoleExistsAsync("Admin"))
            {
                var Role = new IdentityRole()
                {
                    Name = "Admin"

                };
                await _RoleManager.CreateAsync(Role);
            }
            var admin = await _UserManager.FindByEmailAsync("admin@gmail.com");
            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",

                };
                await _UserManager.CreateAsync(admin, "Admin#123");
            }
            if (!await _UserManager.IsInRoleAsync(admin, "Admin"))
            {
               await _UserManager.AddToRoleAsync(admin,"Admin");
            }
        }
    }
}
            