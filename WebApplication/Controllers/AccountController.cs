using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Repository;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserRepository userRepository;
        private readonly UserService _usersService;

        public AccountController(IUserRepository userRepository)
        {
            _usersService = new UserService();
            this.userRepository = userRepository;
        }
        [Route("/Login")]
        public async Task Login(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        [Route("/Logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in the
                // **Allowed Logout URLs** settings for the app.
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        [Authorize]
        [Route("/Account/Info")]
        public async Task<IActionResult> Info()
        {
            string user_id = await GetUserAuth0Id();
            return View("Info");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var auth0Id = await GetUserAuth0Id();
            var user = await userRepository.GetUserByAuth0Id(auth0Id);
            return View(new ProfileViewModel { User = user});
        }

        [Authorize]
        [Route("Account/Courses")]
        public async Task<IActionResult> Courses()
        {
            return View();
        }

        [Authorize]
        [Route("Account/Groups")]
        public async Task<IActionResult> Groups()
        {
            return View();
        }

        [Authorize]
        [Route("Account/Students")]
        public async Task<IActionResult> Students()
        {
            return View();
        }

        [Authorize]
        [Route("Account/Lessons")]
        public async Task<IActionResult> Lessons()
        {
            return View();
        }

        #region Token
        private async Task<string> GetAccessToken()
        {
            if (User.Identity.IsAuthenticated)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                // if you need to check the access token expiration time, use this value
                // provided on the authorization response and stored.
                // do not attempt to inspect/decode the access token
                var accessTokenExpiresAt = DateTime.Parse(
                    await HttpContext.GetTokenAsync("expires_at"),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

                var idToken = await HttpContext.GetTokenAsync("id_token");

                return accessToken;

                // Now you can use them. For more info on when and how to use the 
                // access_token and id_token, see https://auth0.com/docs/tokens
            }
            return string.Empty;

        }
        private Task<string> GetUserAuth0Id()
        {
            return Task.Factory.StartNew(() =>
            {
                return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            });
        }
        #endregion

    }
}