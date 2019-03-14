using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class StudentsViewModel : DashboradLayoutViewModel
    {
        public IEnumerable<Student> Students { get; set; }
    }
}
