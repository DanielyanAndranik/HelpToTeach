using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Helpers;

namespace WebApplication.ViewModels
{
    public class GroupCourseViewModel:DashboradLayoutViewModel
    {
        public List<GroupCourseRow> GroupCourses { get; set; }
    }
}
