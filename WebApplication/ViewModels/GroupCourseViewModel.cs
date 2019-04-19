using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Helpers;

namespace WebApplication.ViewModels
{
    public class GroupCoursesViewModel : DashboradLayoutViewModel
    {
        public List<GroupCourse> GroupCourses { get; set; }
    }
}
