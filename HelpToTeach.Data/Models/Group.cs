using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Data.Models
{
    public class Group : EntityBase<Group>
    {
        public string Name { get; set; }
        //public List<Student> Students { get; set; }
        //public List<GroupCourse> GroupCourses { get; set; }
    }
}
