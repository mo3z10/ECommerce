using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Api.SeedData
{
    public class SeedData
    {
        public static async Task SeedAdmin(UserManager<ApplicationUser> _UserManager, RoleManager<IdentityRole> _RoleManager,IConfiguration configuration)
        {
        
            if (!await _RoleManager.RoleExistsAsync("Admin"))
            {
                var Role = new IdentityRole()
                {
                    Name = "Admin"

                };
                await _RoleManager.CreateAsync(Role);
            }
            var adminEmail = configuration["Admin:AdminMail"];
            var adminPassword = configuration["Admin:AdminPassword"];
            var admin = await _UserManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    UserName = "Admin",
                    Email = adminEmail,

                };
                await _UserManager.CreateAsync(admin, adminPassword);
            }
            if (!await _UserManager.IsInRoleAsync(admin, "Admin"))
            {
               await _UserManager.AddToRoleAsync(admin,"Admin");
            }
        }
    }
}
            