using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Repository;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;
        public AccountController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            var user = await userRepository.Get(id);
            return View(new ProfileViewModel { User = user });
        }
    }
}