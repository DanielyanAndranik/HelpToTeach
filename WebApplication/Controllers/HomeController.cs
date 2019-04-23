using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using HelpToTeach.Core.AI;
using HelpToTeach.Core.Repository;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.Helpers.Enums;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly INamedBucketProvider provider;
        private readonly IUserRepository userRepository;
        private readonly IHostingEnvironment env;
        private readonly IBucket bucket;

        public HomeController(INamedBucketProvider provider,IUserRepository userRepository,IHostingEnvironment env)
        {
            this.provider = provider;
            this.userRepository = userRepository;
            this.env = env;
            this.bucket = provider.GetBucket();
        }
        public async Task<IActionResult> Index()
        {
            //var result = PythonRunner.Run(@"D:\Դիպլոմային\HelpToTeach\HelpToTeach.Core\AI\app.py", "");
            //ViewData.Add("result", result);

            string superUserString = await ReadFile(env.WebRootPath + "//SuperUser.json");
            User superUser = JsonConvert.DeserializeObject<User>(superUserString);
            User user = await userRepository.Get(superUser.Id);
            if (user == null) {
                await userRepository.Create(superUser,"root123");
            }
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login() => View();

        public IActionResult Register() => View(new RegisterViewModel(RegisterMode.Staff));

        public IActionResult ErrorForbidden() => View();

        public IActionResult ErrorNotLoggedin() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Task<string> ReadFile(string FileName)
        {
            return Task.Factory.StartNew(()=> {
                string result;
                try
                {
                    FileStream fileStream = new FileStream(FileName, FileMode.Open);
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    //Log
                    throw ex;
                }
                return result;
            });
        }

    }
}
