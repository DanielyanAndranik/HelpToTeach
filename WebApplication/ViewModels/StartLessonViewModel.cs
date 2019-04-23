using HelpToTeach.Data.Enums;
using HelpToTeach.Data.Models;
using HelpToTeach.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class StartLessonViewModel : DashboradLayoutViewModel
    {
        public LessonType Type { get; set; }
        public GroupCourse GroupCourse { get; set; }
        public Lesson Lesson { get; set; }
        public List<Student> Students { get; set; }
        public List<Mark> Marks { get; set; }
        public List<KeyValuePair<string, int>> PredictedValues { get; set; }
        public string ErrorMessage { get; set; }
    }
}
