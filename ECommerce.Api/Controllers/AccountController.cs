using System.Runtime.CompilerServices;
using ECommerce.BIL.DTOS.AuthenticationDtos.AuthDtos;
using ECommerce.BIL.DTOS.AuthenticationDtos.EmailServicesDtos;
using ECommerce.BIL.Services.AuthenicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        [EnableRateLimiting("Fixed")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {

            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        [HttpPost("Register")]
        [EnableRateLimiting("Fixed")]

        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok();
        }
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _authService.ChangePasswordAsync(dto);


            return result
                ? Ok("Password changed")
                : BadRequest();
        }



        [HttpPost("forgot-password")]
        [EnableRateLimiting("Fixed")]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            
           var result =  await _authService.ForgotPasswordAsync(dto);
            if (!result)
            {
                return BadRequest();
            }
            return Ok("OTP sent");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result =
                await _authService.ResetPasswordAsync(dto);


            return result
                ? Ok("Password reset")
                : BadRequest();
        }
    }
}