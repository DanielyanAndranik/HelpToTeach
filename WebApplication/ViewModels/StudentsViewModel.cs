using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class StudentsViewModel : DashboradLayoutViewModel
    {
        public IEnumerable<Student> Students { get; set; }
    }
}
