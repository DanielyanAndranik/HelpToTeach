﻿using HelpToTeach.Core.Repository;
using HelpToTeach.Data.Enums;
using HelpToTeach.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private readonly ILessonRepository lessonRepository;
        private readonly IMLDataRepository mlDataRepository;

        public DashboardController(
                ICourseRepository courseRepository,
                IGroupRepository groupRepository,
                IStudentRepository studentRepository,
                IUserRepository userRepository,
                IGroupCourseRepository groupCourseRepository,
                IMarkRepository markRepository,
                ILessonRepository lessonRepository,
                IMLDataRepository mlDataRepository)
        {
            this.courseRepository = courseRepository;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
            this.userRepository = userRepository;
            this.groupCourseRepository = groupCourseRepository;
            this.markRepository = markRepository;
            this.lessonRepository = lessonRepository;
            this.mlDataRepository = mlDataRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("Info");
        }

        [Route("info")]
        public async Task<IActionResult> Info()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);

            var user = await userRepository.Get(id);
            var studentsCount = (await studentRepository.GetByLecturer(id)).Count();
            var groupsCount = (await groupRepository.GetByLecturer(id)).Count();
            var coursesCount = (await courseRepository.GetByLecturer(id)).Count();
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

        private async Task<IEnumerable<Course>> GetCoursesByLecturer(string id)
        {
            List<GroupCourse> groupCourses = await groupCourseRepository.GetByLecturer(id);
            List<Course> result = new List<Course>();
            if (groupCourses == null || groupCourses.Count == 0)
            {
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
            return View("EditCourse", new EditCourseViewModel { Mode = Mode.New });
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
            else
            {
                groups = await groupRepository.GetByLecturer(id);
            }

            return View(new GroupsViewModel { Groups = groups, SelectedGroup = new Group() });
        }

        private async Task<List<Group>> GetGroupsByLecturer(string id)
        {
            List<GroupCourse> groupCourses = await groupCourseRepository.GetByLecturer(id);
            List<Group> result = new List<Group>();
            if (groupCourses == null || groupCourses.Count == 0)
            {
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
            return View("EditGroup", new EditGroupViewModel { Mode = Mode.Edit, Group = group });
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

        private async Task<IEnumerable<Student>> GetStudentsByLecturer(string id)
        {
            List<Group> groups = await GetGroupsByLecturer(id);
            List<Student> students = new List<Student>();
            if (groups.Count == 0)
            {
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

        [Route("students/edit")]
        public async Task<IActionResult> EditStudent(string studentId)
        {
            var student = await this.studentRepository.Get(studentId);
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
            student.FullScholarship = _student.FullScholarship;
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
            return View("~/Views/Home/Register.cshtml", new RegisterViewModel(RegisterMode.Lecturer));
        }

        #endregion

        #region GCL
        [Route("groupcourses")]
        public async Task<IActionResult> GroupCourses()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            List<GroupCourse> groupCourses;
            if (role == "1")
            {
                var id = User.FindFirstValue(ClaimTypes.Sid);
                groupCourses = await groupCourseRepository.GetByLecturer(id);
            }
            else
            {
                groupCourses = await groupCourseRepository.GetAll();
            }

            return View(new GroupCoursesViewModel()
            {
                GroupCourses = groupCourses
            });
        }

        [Route("groupcourses/add")]
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
        [Route("groupcourses/add")]
        public async Task<IActionResult> AddGroupCourse([FromForm]GroupCourse groupCourse)
        {
            var _groupCourse = await this.groupCourseRepository.Create(groupCourse);
            return RedirectToAction("GroupCourses");
        }

        #endregion

        #region Lessons

        [Route("lessons")]
        public async Task<IActionResult> Lessons()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var lessons = new List<Lesson>();
            if (role == "1")
            {
                lessons = await lessonRepository.GetByLecturer(id);
            }
            else
            {
                lessons = await lessonRepository.GetAll();
            }

            foreach (var lesson in lessons)
            {
                lesson.GroupCourse = await groupCourseRepository.Get(lesson.GroupCourseId);
            }

            return View(new LessonsViewModel { Lessons = lessons });
        }

        [Route("lessons/start")]
        public async Task<IActionResult> StartLesson(string groupCourseId, LessonType type)
        {
            var lesson = await lessonRepository.Create(new Lesson
            {
                GroupCourseId = groupCourseId,
                LessonType = type,
                Date = DateTime.Now
            });

            var groupCourse = await groupCourseRepository.Get(groupCourseId);
            var students = await studentRepository.GetByGroupId(groupCourse.GroupId);
            var marks = new List<Mark>();

            foreach (var student in students)
            {
                marks.Add(new Mark
                {
                    StudentId = student.Id,
                    MarkType = (type == LessonType.Lecture || type == LessonType.Seminar) ? MarkType.Activity : (MarkType)((int)type - 1)
                });
            }

            var viewModel = new StartLessonViewModel { GroupCourse = groupCourse, Type = type, Students = students, Marks = marks };

            var result = new KeyValuePair<bool, List<Mark>>();

            if (type == LessonType.FirstMiddle)
            {
                result = await mlDataRepository.GetFirstMiddlePrediction(groupCourseId);
            }
            else if (type == LessonType.SecondMiddle)
            {
                result = await mlDataRepository.GetSecondMiddlePrediction(groupCourseId);
            }
            else if (type == LessonType.Final)
            {
                result = await mlDataRepository.GetFinalPrediction(groupCourseId);
            }

            if (!result.Key)
                viewModel.ErrorMessage = "Error occured when trying to predict marks";
            else
                viewModel.PredictedMarks = result.Value;
            return View("AddLesson", viewModel);
        }

        [HttpPost]
        [Route("lessons/save")]
        public async Task<IActionResult> SaveLesson(StartLessonViewModel startLessonViewModel)
        {
            startLessonViewModel.Marks.ForEach(m => m.LessonId = startLessonViewModel.Lesson.Id);

            var lesson = await lessonRepository.Get(startLessonViewModel.Lesson.Id);
            lesson.Saved = true;
            await lessonRepository.Update(lesson);

            await markRepository.AddRange(startLessonViewModel.Marks);

            return RedirectToAction("Lessons");
        }

        [HttpPost]
        [Route("lessons/cancel")]
        public async Task<IActionResult> CancelLesson(StartLessonViewModel startLessonViewModel)
        {
            await lessonRepository.Delete(startLessonViewModel.Lesson.Id);
            await markRepository.DeletePredictedMarksByLesson(startLessonViewModel.Lesson.Id);
            return RedirectToAction("Lessons");
        }

        #endregion

        #region Requests

        [Route("registrationrequests")]
        public async Task<IActionResult> RegistrationRequests()
        {
            List<User> users = await userRepository.GetAll();
            users = (from u in users where u.Approved == false select u).ToList();
            return View(new RegistrationRequestsViewModel()
            {
                Users = users
            });
        }

        [Route("registrationrequests/accept/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> AcceptRegistration([FromRoute] string id)
        {
            try
            {
                var user = await userRepository.Get(id);
                user.Approved = true;
                await userRepository.Update(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            return RedirectToAction("RegistrationRequests");
        }

        [Route("registrationrequests/deny/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DenyRegistration([FromRoute] string id)
        {
            try
            {
                await userRepository.Delete(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            return RedirectToAction("RegistrationRequests");
        }

        #endregion

        #region Info

        [Route("StudentInfo/{id}")]
        public async Task<IActionResult> StudentInfo([FromRoute]string id)
        {
            Student student = await studentRepository.Get(id);
            student.Group = await groupRepository.Get(student.GroupId);

            List<Mark> studentMarks = await markRepository.GetMarksByStudent(student.Id);

            for (int i = 0; i < studentMarks.Count; i++)
            {
                if (studentMarks[i].CourseId != null)
                {
                    studentMarks[i].Course = await courseRepository.Get(studentMarks[i].CourseId);
                }
                else {
                    Lesson temp = await lessonRepository.Get(studentMarks[i].LessonId);
                    GroupCourse gc = await groupCourseRepository.Get(temp.GroupCourseId);
                    studentMarks[i].Course = await courseRepository.Get(gc.CourseId);
                }
            }

            return View(new StudentInfoViewModel() {
                Student = student,
                Marks = studentMarks
            });
        }

        [Route("UserInfo/{id}")]
        public async Task<IActionResult> UserInfo([FromRoute]string id)
        {
            return View();
        }

        #endregion

    }
}