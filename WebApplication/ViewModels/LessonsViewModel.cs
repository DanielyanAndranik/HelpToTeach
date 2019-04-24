using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Helpers;

namespace WebApplication.ViewModels
{
    public class LessonsViewModel : DashboradLayoutViewModel
    {
        public List<Lesson> Lessons { get; set; }
    }
}
