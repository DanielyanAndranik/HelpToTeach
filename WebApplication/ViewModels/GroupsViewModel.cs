using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class GroupsViewModel : DashboradLayoutViewModel
    {
        public IEnumerable<Group> Groups { get; set; }

        public Group SelectedGroup { get; set; }
    }
}
