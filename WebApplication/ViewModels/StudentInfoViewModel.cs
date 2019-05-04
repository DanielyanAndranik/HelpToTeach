using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class StudentInfoViewModel:DashboradLayoutViewModel
    {
        public Student Student { get; set; }
        public List<Mark> Marks { get; set; }
    }
}
