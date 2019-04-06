using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class TeachersViewModel:DashboradLayoutViewModel
    {
        public List<User> Teachers { get; set; }
    }
}
