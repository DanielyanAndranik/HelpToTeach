using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Data.Models
{
    public class GroupCourse : EntityBase<GroupCourse>
    {
        [Required,Display(Name ="Group Name")]
        public string GroupId { get; set; }
        [Required,Display(Name ="Course Name")]
        public string CourseId { get; set; }
        [Required,Display(Name ="Teacher Name")]
        public string UserId { get; set; }
    }
}
