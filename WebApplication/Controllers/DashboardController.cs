using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpToTeach.Core.Repository;
using HelpToTeach.Core.Repository.Abstraction;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Helpers;
using WebApplication.Helpers.Enums;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IUserRepository userRepository;
        private readonly IGroupCourseRepository groupCourseRepository;
        private readonly IMarkRepository markRepository;


        public DashboardController(ICourseRepository courseRepository,
            IGroupRepository groupRepository,
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IGroupCourseRepository groupCourseRepository,
            IMarkRepository markRepository)
        {
            this.courseRepository = courseRepository;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
            this.userRepository = userRepository;
            this.groupCourseRepository = groupCourseRepository;
            this.markRepository = markRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("Info");
        }

        [Route("info")]
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
        [Route("courses")]
        public async Task<IActionResult> Courses()
        {
            IEnumerable<Course> courses;
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            if (User.FindFirst(ClaimTypes.Role).Value == "1")
            {
                courses = await courseRepository.GetByLecturer(id);
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

        [Route("courses/add")]
        public IActionResult AddCourse()
        {
            return View("EditCourse", new EditCourseViewModel { Mode = Mode.New});
        }

        [HttpPost]
        [Route("courses/add")]
        public async Task<IActionResult> AddCourse([FromForm] string name)
        {
            var course = await this.courseRepository.Create(new Course { Name = name });
            return RedirectToAction("Courses");
        }

        [Route("courses/edit/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> EditCourse(string id)
        {
            var course = await this.courseRepository.Get(id);
            return View("EditCourse", new EditCourseViewModel { Mode = Mode.Edit, Course = course });
        }

        [HttpPost]
        [Route("courses/edit/{id}")]
        public async Task<IActionResult> EditCourse([FromRoute] string id, [FromForm] Group _course)
        {
            var course = await courseRepository.Get(id);
            course.Name = _course.Name;
            await this.courseRepository.Update(course);
            return RedirectToAction("Courses");
        }

        [Route("courses/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            await this.courseRepository.Delete(id);
            return RedirectToAction("Courses");
        }
        #endregion

        #region Group

        [Route("groups")]
        [HttpGet]
        public async Task<IActionResult> Groups()
        {
            IEnumerable<Group> groups;
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            if (!(User.FindFirst(ClaimTypes.Role).Value == "1"))
            {
                groups = await groupRepository.GetAll();
            }
            else {
                groups = await groupRepository.GetByLecturer(id);
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

        [Route("groups/add")]
        [HttpGet]
        public IActionResult AddGroup()
        {
            return View("EditGroup", new EditGroupViewModel { Mode = Mode.New, Group = new Group() });
        }

        [HttpPost]
        [Route("groups/add")]
        public async Task<IActionResult> AddGroup([FromForm] Group group)
        {
            await this.groupRepository.Create(new Group { Name = group.Name });
            return RedirectToAction("Groups");
        }

        [Route("groups/edit/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> EditGroup(string id)
        {
            var group = await this.groupRepository.Get(id);
            return View("EditGroup", new EditGroupViewModel { Mode = Mode.Edit, Group = group});
        }

        [HttpPost]
        [Route("groups/edit/{id}")]
        public async Task<IActionResult> EditGroup([FromRoute] string id, [FromForm] Group _group)
        {
            var group = await groupRepository.Get(id);
            group.Name = _group.Name;
            await this.groupRepository.Update(group);
            return RedirectToAction("Groups");
        }

        [Route("groups/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            await this.groupRepository.Delete(id);
            return RedirectToAction("Groups");
        }
        #endregion

        #region Students
        [Route("students")]
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
                students = await studentRepository.GetByLecturer(id);
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

        [Route("students/add")]
        public async Task<IActionResult> AddStudent()
        {
            var groups = await this.groupRepository.GetAll();
            return View("EditStudent", new EditStudentViewModel { Mode = Mode.New, Groups = groups });
        }

        [HttpPost]
        [Route("students/add")]
        public async Task<IActionResult> AddStudent([FromForm] Student student)
        {
            var _student = await this.studentRepository.Create(student);
            return RedirectToAction("Students");
        }

        [Route("students/edit/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> EditStudent(string id)
        {
            var student = await this.studentRepository.Get(id);
            var groups = await this.groupRepository.GetAll();
            return View("EditStudent", new EditStudentViewModel { Mode = Mode.Edit, Student = student, Groups = groups });
        }

        [HttpPost]
        [Route("students/edit/{id}")]
        public async Task<IActionResult> EditStudent([FromRoute] string id, [FromForm] Student _student)
        {
            var student = await studentRepository.Get(id);
            student.FirstName = _student.FirstName;
            student.LastName = _student.LastName;
            student.MiddleName = _student.MiddleName;
            student.GroupId = _student.GroupId;
            student.FullSchoolarship = _student.FullSchoolarship;
            student.BirthDate = _student.BirthDate;
            await this.studentRepository.Update(student);
            return RedirectToAction("Students");
        }

        [Route("students/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            await this.studentRepository.Delete(id);
            return RedirectToAction("Students");
        }

        #endregion

        #region Lecturers
        [Route("lecturers")]
        public async Task<IActionResult> Lecturers()
        {
            var result = await userRepository.GetLecturers();
            return View(new LecturersViewModel()
            {
                Lecturers = result
            });
        }

        [Route("lecturers/add")]
        public async Task<IActionResult> AddLecturer()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GCL
        [Route("gropucourses")]
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

        

        [Route("gropucourses/add")]
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
        [Route("gropucourses/add")]
        public async Task<IActionResult> AddGroupCourse([FromForm]GroupCourse groupCourse)
        {
            var _groupCourse = await this.groupCourseRepository.Create(groupCourse);
            return RedirectToAction("GroupCourse");
        }

        #endregion

        #region Lessons

        [Route("gropucourses/MyLessons")]
        public async Task<IActionResult> MyLessons()
        {
            var id = User.FindFirst(ClaimTypes.Sid).Value;

            List<GroupCourse> groupCourses = await groupCourseRepository.GetByLecturerId(id);

            if (groupCourses == null)
            {
                groupCourses = new List<GroupCourse>();
            }

            List<GroupCourseRow> result = new List<GroupCourseRow>();

            for (int i = 0; i < groupCourses.Count; i++)
            {
                Group group = await groupRepository.Get(groupCourses[i].GroupId);
                Course course = await courseRepository.Get(groupCourses[i].CourseId);
                User lecturer = await userRepository.GetLecturerById(groupCourses[i].UserId);
                result.Add(new GroupCourseRow()
                {
                    GroupCourseId = groupCourses[i].Id,
                    GroupName = group.Name,
                    CourseName = course.Name,
                    LecturerName = lecturer.FirstName
                });
            }

            return View(new MyLessonsViewModel()
            {
                GroupCourses = result
            });
        }

        [Route("gropucourses/ShowMore")]
        public async Task<IActionResult> ShowMore(string groupCourseId) {
            GroupCourse gc = await groupCourseRepository.Get(groupCourseId);
            if (gc == null) {
                throw new NotImplementedException();
            }

            List<Student> students = await studentRepository.GetByGroupId(gc.GroupId);

            return View(new ShowMoreViewModel() {
                Students = students,
                GroupCourseId = groupCourseId
            });
        }

        [Route("gropucourses/SetMark")]
        public async Task<IActionResult> SetMark(string studentId,string groupCourseId) {
            Mark mark = new Mark();
            mark.GroupCourseId = groupCourseId;
            mark.StudentId = studentId;

            Student student = await studentRepository.Get(studentId);
            GroupCourse gc = await groupCourseRepository.Get(groupCourseId);
            Group group = await groupRepository.Get(gc.GroupId);
            Course course = await courseRepository.Get(gc.CourseId);

            return View(new SetMarkViewModel() {
                Student = student,
                Mark = mark,
                Group = group,
                Course = course
            });
        }

        [HttpPost]
        [Route("gropucourses/AddMark")]
        public async Task<IActionResult> AddMark([FromForm]Mark mark) {
            await markRepository.Create(mark);
            return RedirectToAction("MyLessons");
        }

        #endregion

    }
}