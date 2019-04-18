using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class ShowMoreViewModel:DashboradLayoutViewModel
    {
        public string GroupCourseId { get; set; }
        public List<Student> Students { get; set; }
    }
}
