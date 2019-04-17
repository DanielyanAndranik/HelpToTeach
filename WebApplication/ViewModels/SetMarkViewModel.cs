using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class SetMarkViewModel:DashboradLayoutViewModel
    {
        public Student Student { get; set; }
        public Mark Mark { get; set; }
        public Group Group { get; set; }
        public Course Course { get; set; }
    }
}
