using System;
using System.Threading.Tasks;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using HelpToTeach.Data.Transfer;
using HelpToTeach.Core.Repository;

namespace WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]UserDto userDto)
        {
            var user = await repository.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, user.Role),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            if (user.Approved) {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]UserDto userDto)
        {
            // map dto to entity
            var user = mapper.Map<User>(userDto);

            try
            {
                //// save 
                await repository.Create(user, userDto.Password);
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, $"user::{user.Id}"),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Role, user.Role),
                }, 
                CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                if (user.Approved)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}