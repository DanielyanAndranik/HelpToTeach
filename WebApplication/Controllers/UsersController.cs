using System;
using System.Threading.Tasks;
using WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]UserDto userDto)
        {
            // map dto to entity
            var user = mapper.Map<User>(userDto);

            try
            {
                //// save 
                await repository.AddUser(user, userDto.Password);
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        //#region GET
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var result = await _usersRepository.GetAll(typeof(User));
        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e);
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById([FromRoute]string id)
        //{
        //    try
        //    {
        //        User user = await _usersRepository.Get(id);
        //        if (user == null) { return NotFound(); }
        //        return Ok(user);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e);
        //    }
        //}

        ////[HttpGet("by_auth0_id/{id}")]
        ////public async Task<IActionResult> GetByAuth0Id([FromRoute]string id)
        ////{
        ////    try
        ////    {
        ////        List<User> users = await _usersRepository.GetAll(typeof(User));
        ////        User user = users.FirstOrDefault(u => u.Auth0Id == id);
        ////        if (user == null) { return NotFound(); }
        ////        return Ok(user);
        ////    }
        //    //catch (Exception e)
        ////    {
        ////        return StatusCode(500,e);
        ////    }
        ////}

        //#endregion

        //#region POST

        //[HttpPost("Create")]
        //public async Task<IActionResult> Create([FromBody]User user)
        //{
        //    try
        //    {
        //        var result = await _usersRepository.Create(user);
        //        if (result == null) { return BadRequest(); }
        //        return Created($"/api/Users/{user.Id}", result);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500,e);
        //    }
        //}

        //#endregion

        //#region PUT

        //[HttpPut("Update")]
        //public async Task<IActionResult> Update([FromBody]User user)
        //{
        //    try
        //    {
        //        var result = await _usersRepository.Update(user);
        //        if (result == null) { return BadRequest(result); }
        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500,e);
        //    }
        //}

        //#endregion

        //#region DELETE

        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> Delete([FromRoute]string id)
        //{
        //    try
        //    {
        //        await _usersRepository.Delete(id);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e);
        //    }
        //}

        //#endregion


    }
}