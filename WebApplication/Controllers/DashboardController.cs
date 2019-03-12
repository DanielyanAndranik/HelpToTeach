using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Repository;

namespace WebApplication.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly IGroupRepository groupRepository;
        public DashboardController(ICourseRepository courseRepository, IGroupRepository groupRepository)
        {
            this.courseRepository = courseRepository;
            this.groupRepository = groupRepository;
        }
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        #region Course
        public async Task<IActionResult> Courses()
        {
            if(User.FindFirst(ClaimTypes.Role).Value == "Lecturer")
            {
                var id = User.FindFirst(ClaimTypes.Sid).Value;
                var courses = await this.courseRepository.GetByLecturer(id);
            }
            return View();

        }

        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromForm] string name)
        {
            var course = await this.courseRepository.Create(new Course { Name = name });
            return RedirectToAction("Courses");
        }
        #endregion

        #region Group

        public async Task<IActionResult> Groups()
        {
            return View();
        }

        public IActionResult AddGroup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromForm] string name)
        {
            var group = await this.groupRepository.Create(new Group { Name = name });
            return RedirectToAction("Groups");
        }

        #endregion

        #region Students
        public async Task<IActionResult> Students()
        {
            return View();
        }

        #endregion  
    }
}