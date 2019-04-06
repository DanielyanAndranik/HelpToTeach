using HelpToTeach.Data.Models;

namespace WebApplication.ViewModels
{
    public class InfoViewModel : DashboradLayoutViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalGroups { get; set; }
    }
}
