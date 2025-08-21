using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperStoreEcommerceAPI.DTOs;
using SuperStoreEcommerceAPI.Models;
using SuperStoreEcommerceAPI.Services;
using System.ComponentModel;
using System.Net.Mime;

namespace SuperStoreEcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                Email = req.Email,
                UserName = req.Email,
                FullName = req.FullName
            };

            var result = await _userManager.CreateAsync(user, req.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            // default role is Customer
            var role = string.IsNullOrWhiteSpace(req.Role) ? "Customer" : req.Role;
            if (!await _roleManager.RoleExistsAsync(role)) role = "Customer";
            await _userManager.AddToRoleAsync(user, role);

            var roles = await _userManager.GetRolesAsync(user);
            var (token, exp) = _tokenService.CreateToken(user, roles);

            return Ok(new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = exp,
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null) return Unauthorized("Invalid credentials.");

            var check = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);
            if (!check.Succeeded) return Unauthorized("Invalid credentials.");

            var roles = await _userManager.GetRolesAsync(user);
            var (token, exp) = _tokenService.CreateToken(user, roles);

            return Ok(new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = exp,
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles
            });
        }
    }
}
