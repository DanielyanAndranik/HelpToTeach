using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class EditStudentViewModel : EditViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<Group> Groups { get; set; }
    }
}
