using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class LecturersViewModel : DashboradLayoutViewModel
    {
        public List<User> Lecturers { get; set; }
    }
}
