using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class GroupCourse : EntityBase<GroupCourse>
    {
        public string GroupId { get; set; }
        public string CourseId { get; set; }
        public string UserId { get; set; }
    }
}
