using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpToTeach.Core.Repository;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Helpers;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IUserRepository userRepository;
        private readonly IGroupCourseRepository groupCourseRepository;


        public DashboardController(ICourseRepository courseRepository,
            IGroupRepository groupRepository,
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IGroupCourseRepository groupCourseRepository)
        {
            this.courseRepository = courseRepository;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
            this.userRepository = userRepository;
            this.groupCourseRepository = groupCourseRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Info");
        }

        public async Task<IActionResult> Info()
        {
            var user = await userRepository.Get(User.FindFirstValue(ClaimTypes.Sid));
            var studentsCount = (await studentRepository.GetByLecturer(User.FindFirstValue(ClaimTypes.Sid))).Count();
            var groupsCount = (await groupRepository.GetByLecturer(User.FindFirstValue(ClaimTypes.Sid))).Count();
            var coursesCount = (await courseRepository.GetByLecturer(User.FindFirstValue(ClaimTypes.Sid))).Count();
            return View(new InfoViewModel
            {
                User = user,
                TotalStudents = studentsCount,
                TotalGroups = groupsCount,
                TotalCourses = coursesCount
            });
        }

        #region Course
        public async Task<IActionResult> Courses()
        {
            IEnumerable<Course> courses;
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            if (User.FindFirst(ClaimTypes.Role).Value == "1")
            {
                courses = await GetCoursesByLecturer(id.Split("::")[1]);
            }
            else
            {
                courses = await courseRepository.GetAll();
            }
            return View(new CoursesViewModel { Courses = courses });
        }

        private async Task<IEnumerable<Course>> GetCoursesByLecturer(string id) {
            List<GroupCourse> groupCourses = await groupCourseRepository.GetByLecturerId(id);
            List<Course> result = new List<Course>();
            if (groupCourses == null || groupCourses.Count == 0) {
                return result;
            }

            for (int i = 0; i < groupCourses.Count; i++)
            {
                Course course = await courseRepository.Get(groupCourses[i].CourseId);
                result.Add(course);
            }
            result.GroupBy(x => x.Id).Select(x => x.First());
            return result;
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
            IEnumerable<Group> groups;
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            if (!(User.FindFirst(ClaimTypes.Role).Value == "1"))
            {
                groups = await groupRepository.GetAll();
            }
            else {
                groups = await GetGroupsByLecturer(id.Split("::")[1]);
            }

            return View(new GroupsViewModel { Groups = groups, SelectedGroup = new Group() });
        }

        private async Task<List<Group>> GetGroupsByLecturer(string id) {
            List<GroupCourse> groupCourses = await groupCourseRepository.GetByLecturerId(id);
            List<Group> result = new List<Group>();
            if (groupCourses == null || groupCourses.Count == 0) {
                return result;
            }

            for (int i = 0; i < groupCourses.Count; i++)
            {
                Group group = await groupRepository.Get(groupCourses[i].GroupId);
                result.Add(group);
            }

            result.GroupBy(x => x.Id).Select(x => x.First());

            return result;
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

        public IActionResult EditGroup()
        {
            return View("EditGroup");
        }
        #endregion

        #region Students
        public async Task<IActionResult> Students()
        {
            IEnumerable<Student> students;
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            if (!(User.FindFirst(ClaimTypes.Role).Value == "1"))
            {
                students = await studentRepository.GetAll();
            }
            else
            {
                students = await GetStudentsByLecturer(id.Split("::")[1]);
            }
            return View(new StudentsViewModel { Students = students });
        }

        private async Task<IEnumerable<Student>> GetStudentsByLecturer(string id) {
            List<Group> groups = await GetGroupsByLecturer(id);
            List<Student> students = new List<Student>();
            if (groups.Count == 0) {
                return students;
            }

            for (int i = 0; i < groups.Count; i++)
            {
                List<Student> temp = await studentRepository.GetByGroupId(groups[i].Id);
                foreach (var item in temp)
                {
                    students.Add(item);
                }
            }
            return students;
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

        #region Lecturers

        public async Task<IActionResult> Lecturers()
        {
            var result = await userRepository.GetLecturers();
            return View(new LecturersViewModel()
            {
                Lecturers = result
            });
        }

        public async Task<IActionResult> AddLecturer()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GCL

        public async Task<IActionResult> GroupCourse()
        {
            List<GroupCourse> groupCourses = await groupCourseRepository.GetAll();

            if (groupCourses == null) {
                groupCourses = new List<GroupCourse>();
            }

            List<GroupCourseRow> result = new List<GroupCourseRow>();

            for (int i = 0;i < groupCourses.Count;i++)
            {
                Group group = await groupRepository.Get(groupCourses[i].GroupId);
                Course course = await courseRepository.Get(groupCourses[i].CourseId);
                User lecturer = await userRepository.GetLecturerById(groupCourses[i].UserId);
                result.Add(new GroupCourseRow()
                {
                    GroupName = group.Name,
                    CourseName = course.Name,
                    LecturerName = lecturer.FirstName
                });
            }

            return View(new GroupCourseViewModel()
            {
                GroupCourses = result
            });
        }

        public async Task<IActionResult> AddGroupCourse()
        {
            List<Group> groups = await groupRepository.GetAll();
            List<Course> courses = await courseRepository.GetAll();
            List<User> lecturers = await userRepository.GetLecturers();

            return View(new AddGroupCourseViewModel()
            {
                Groups = groups,
                Courses = courses,
                Lecturers = lecturers,
                GroupCourse = new GroupCourse()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddGroupCourse([FromForm]GroupCourse groupCourse)
        {
            var _groupCourse = await this.groupCourseRepository.Create(groupCourse);
            return RedirectToAction("GroupCourse");
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