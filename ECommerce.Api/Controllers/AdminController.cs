using ECommerce.BIL.DTOS.AuthenticationDtos.AuthDtos;
using ECommerce.BIL.DTOS.AuthenticationDtos.EmailServicesDtos;
using ECommerce.BIL.Services.AuthenicationServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
   [Authorize(Roles = "Admin")]

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Role")]
        public async Task<ActionResult> CreateRole(AddRoleDto dto)
        {
            var result = await _authService.CreateRoleAsync(dto);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }
        [HttpGet("Roles")]
        public async Task<ActionResult> GetRoles()
        {
            var roles = await _authService.GetAllRolesAsync();

            return Ok(roles);
        }
        [HttpGet("Role/{roleName}")]
        public async Task<ActionResult> GetRole(string roleName)
        {
            var role = await _authService.GetRoleByName(roleName);

            if (role == null)
                return NotFound();

            return Ok(role);
        }
        [HttpDelete("Role/{roleId}")]
        public async Task<ActionResult> DeleteRole(string roleId)
        {
            var result = await _authService.DeleteRoleAsync(roleId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpGet("Users")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _authService.GetAllUsersAsync();

            return Ok(users);
        }
        [HttpGet("User/{userId}")]
        public async Task<ActionResult> GetUser(string userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpDelete("User/{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var result = await _authService.DeleteUserASync(userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost("AssignRole")]
        public async Task<ActionResult> AssignRole(
    AssignRoleDto dto)
        {
            var result =
                await _authService.AssignRoleToAsync(dto);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }


    }

}

