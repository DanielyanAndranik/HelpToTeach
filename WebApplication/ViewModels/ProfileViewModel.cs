using HelpToTeach.Data.Models;

namespace WebApplication.ViewModels
{
    public class ProfileViewModel : DashboradLayoutViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalGroups { get; set; }
    }
}
