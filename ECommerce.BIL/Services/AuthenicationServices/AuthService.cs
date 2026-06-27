using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.AuthenticationDtos.AuthDtos;
using ECommerce.BIL.DTOS.AuthenticationDtos.EmailServicesDtos;
using ECommerce.BIL.Services.JobSercvices;
using ECommerce.BIL.Services.NotificationHubService;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;

namespace ECommerce.BIL.Services.AuthenicationServices
{
    public class AuthService : IAuthService
    {
        private readonly INotificationService _NotificaitonService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _EmailService;
        private readonly IJobService _JobService;

        public AuthService
     (INotificationService NotificaitonService, IConfiguration configuration,RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> usermanager,IHttpContextAccessor httpContextAccessor,IEmailService emailService,IUnitOfWork unitOfWork,IJobService jobService)
        {
            _NotificaitonService = NotificaitonService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _usermanager = usermanager;
            _RoleManager=RoleManager;
            _httpContextAccessor=httpContextAccessor;
            _EmailService=emailService;
            _JobService =jobService;
        }

        public async Task<string> AssignRoleToAsync(AssignRoleDto assignRoleDto)
        {
            var RoleExits = await _RoleManager.RoleExistsAsync(assignRoleDto.RoleName);
            var User = await _usermanager.FindByIdAsync(assignRoleDto.UserId);
            if (RoleExits && User!= null ) {
                var oldRoles = await _usermanager.GetRolesAsync(User);


                await _usermanager.RemoveFromRolesAsync(
                    User,
                    oldRoles
                );
                var result = await _usermanager.AddToRoleAsync(User, assignRoleDto.RoleName);
            if (result.Succeeded)
                {

                    var roles = await _usermanager.GetRolesAsync(User);
                    var claims = new List<Claim>
    {
                   new Claim(ClaimTypes.Name, User.UserName),
                   new Claim(ClaimTypes.Email, User.Email),
                   new Claim(
    ClaimTypes.NameIdentifier,
    User.Id
)
    };
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    return "Role Assigned";
                }
                return null;
            
            }
            return null;
                }


        public async Task<string> CreateRoleAsync(AddRoleDto addRoleDto)
        {
            var Role = new IdentityRole()
            {
                Name = addRoleDto.RoleName,
                NormalizedName = addRoleDto.RoleName.ToUpper()

            };

            var result = await _RoleManager.CreateAsync(Role);
                if (result.Succeeded)
            {
                return "Created";
            }
            return null;
        }

        public async Task<string> DeleteRoleAsync(string RoleId)
        {
            var role = await _RoleManager.FindByIdAsync(RoleId);
            if (role != null)
            {
          var result = await _RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return "Deleted";
                }
                return null ;
            }
            return null;
        }

        public async Task<string> DeleteUserASync(string UserId)
        {
            var User = await _usermanager.FindByIdAsync(UserId);
            if (User != null) { 
            var result  = await _usermanager.DeleteAsync(User);
            if (result.Succeeded)
                {
                    return "Deleted";
                }
                return null;
            }
            return null;

        }


        public async Task<ICollection<ReadRoleDto>> GetAllRolesAsync()
        {
            var Roles = await _RoleManager.Roles.Select(S => new ReadRoleDto()
            {
                Id = S.Id,
                RoleName = S.Name
            }).ToListAsync();
            return Roles;
        }

        public async Task<ICollection<ReadUserDto>> GetAllUsersAsync()
        {
            var users = await _usermanager.Users.ToListAsync();

            var result = new List<ReadUserDto>();

            foreach (var user in users)
            {
                var roles = await _usermanager.GetRolesAsync(user);

                result.Add(new ReadUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return result;
        }
        public async Task<ReadRoleDto> GetRoleByName(string RoleName)
        {
            var Role = await _RoleManager.Roles.Where(a=> a.Name == RoleName).FirstOrDefaultAsync();
            if (Role != null)
            {
                return new ReadRoleDto()
                {
                    Id = Role.Id,
                    RoleName = Role.Name
                };
            }
          return null ;
        }

        public async Task<ReadUserDto> GetUserByIdAsync(String UserId)
        { 
            var User = await _usermanager.FindByIdAsync(UserId);
            var Role = await _usermanager.GetRolesAsync(User);
            if (User != null)
            {
                return new ReadUserDto()
                {
                    Id = User.Id,
                    UserName = User.UserName
                    ,
                    Roles = Role
                };
            }
            return null ;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _usermanager.FindByEmailAsync(registerDto.Email);

            if (existingUser != null)
                throw new Exception("Email already exists");


            var customer = new Customer()
            {
                UserName = registerDto.Name,
                Address = registerDto.address,
                PhoneNumber = registerDto.phonenumber,

                cart = new Cart()
            };


            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.Name,
                Customer = customer
            };


            if (!await _RoleManager.RoleExistsAsync("Customer"))
            {
                await _RoleManager.CreateAsync(new IdentityRole("Customer"));
            }


            var result = await _usermanager.CreateAsync(user, registerDto.Password);


            if (result.Succeeded)
            {
                await _usermanager.AddToRoleAsync(user, "Customer");
                _JobService.ApplyWelcomeEmail(user.Email);
                await _NotificaitonService.NewCustomerRegistered(user.UserName);
            }


            return result;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _usermanager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                throw new UnauthorizedAccessException();


            var valid = await _usermanager.CheckPasswordAsync(
                user,
                loginDto.Password
            );


            if (!valid)
                throw new UnauthorizedAccessException();


            var roles = await _usermanager.GetRolesAsync(user);


            var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id
        ),

        new Claim(
            ClaimTypes.Name,
            user.UserName
        ),

        new Claim(
            ClaimTypes.Email,
            user.Email
        )
    };


            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role
                    )
                );
            }


            return GenerateToken(claims);
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
                return false;


            var user = await _usermanager.FindByIdAsync(userId);


            if (user == null)
                return false;


            var result = await _usermanager.ChangePasswordAsync(
                user,
                dto.OldPassword,
                dto.NewPassword
            );
            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => e.Description);

                throw new Exception(string.Join(", ", errors));
            }

            return result.Succeeded;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _usermanager.FindByEmailAsync(dto.Email);


            if (user == null)
                return false;


            var otp = Random.Shared.Next(100000, 999999)
                .ToString();


            user.ResetCode = otp;
            user.ResetCodeExpire = DateTime.UtcNow.AddMinutes(10);


            await _usermanager.UpdateAsync(user);


            await _EmailService.SendAsync(
                user.Email,
                "Reset password OTP",
                $"Your OTP is {otp}"
            );


            return true;
        }


        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user =
                    await _usermanager.FindByEmailAsync(dto.Email);


            if (user == null)
                return false;


            if (user.ResetCode != dto.OTP ||
               user.ResetCodeExpire < DateTime.UtcNow)
            {
                return false;
            }



            var token =
                await _usermanager.GeneratePasswordResetTokenAsync(user);



            var result =
                await _usermanager.ResetPasswordAsync(
                    user,
                    token,
                    dto.NewPassword
                );



            if (result.Succeeded)
            {
                user.ResetCode = null;
                user.ResetCodeExpire = null;

                await _usermanager.UpdateAsync(user);
            }


            return result.Succeeded;
        }


        private string GenerateToken(IList<Claim> claims)
        {
            var key = _configuration["JWT:Key"].ToString();
            var EKey = Encoding.UTF8.GetBytes(key);
            var secretKey = new SymmetricSecurityKey(EKey);
            var Cred = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.UtcNow.AddDays(2);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims :claims,expires:expire,signingCredentials:Cred);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(jwtSecurityToken);
            return token;

        }
    }
}
