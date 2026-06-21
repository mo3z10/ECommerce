using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BIL.DTOS.AuthenticationDtos.AuthDtos;
using ECommerce.BIL.DTOS.AuthenticationDtos.EmailServicesDtos;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.BIL.Services.AuthenicationServices
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<string> CreateRoleAsync (AddRoleDto  addRoleDto);
        Task<string> AssignRoleToAsync(AssignRoleDto assignRoleDto);
        Task<ICollection<ReadRoleDto>> GetAllRolesAsync();
        Task<ICollection<ReadUserDto>> GetAllUsersAsync();
        Task <ReadUserDto> GetUserByIdAsync(string UserId);
        Task<ReadRoleDto> GetRoleByName(string RoleName);
        Task<string> DeleteUserASync(string UserID);
        Task<string> DeleteRoleAsync(string RoleId);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);



    }
}
