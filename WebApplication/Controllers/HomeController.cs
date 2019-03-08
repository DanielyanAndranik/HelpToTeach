using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHelpToTeachBucketProvider provider;
        private readonly IBucket bucket;

        public HomeController(IHelpToTeachBucketProvider provider)
        {
            this.provider = provider;
            this.bucket = provider.GetBucket();
        }
        public IActionResult Index() => View();

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login() => View();

        public IActionResult Register() => View();

        public IActionResult ErrorForbidden() => View();

        public IActionResult ErrorNotLoggedin() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
