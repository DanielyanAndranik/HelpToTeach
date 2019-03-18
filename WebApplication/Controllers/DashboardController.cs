using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Repository;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IStudentRepository studentRepository;

        public DashboardController(ICourseRepository courseRepository, IGroupRepository groupRepository, IStudentRepository studentRepository)
        {
            this.courseRepository = courseRepository;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Course
        public async Task<IActionResult> Courses()
        {
            if (User.FindFirst(ClaimTypes.Role).Value == "Lecturer")
            {
                var id = User.FindFirst(ClaimTypes.Sid).Value;
                var _courses = await this.courseRepository.GetByLecturer(id);
            }

            var courses = await this.courseRepository.GetAll();
            return View(new CoursesViewModel { Courses = courses });
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
            var groups = await this.groupRepository.GetAll();
            return View(new GroupsViewModel { Groups = groups });
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
            var students = await this.studentRepository.GetAll();
            return View(new StudentsViewModel { Students = students });
        }

        public async Task<IActionResult> AddStudent()
        {
            var groups = await this.groupRepository.GetAll();
            return View(new AddStudentViewModel { Groups = groups, Student = new Student() });
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] Student student)
        {
            var _student = await this.studentRepository.Create(student);
            return RedirectToAction("Students");
        }

        #endregion  

        public IActionResult Assign([FromQuery] string GroupId)
        {
            if (string.IsNullOrWhiteSpace(GroupId))
            {
                throw new System.ArgumentException("message", nameof(GroupId));
            }

            return View();
        }
    }
}