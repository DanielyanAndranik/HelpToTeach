using HelpToTeach.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HelpToTeach.Data.Models
{
    public class Mark : EntityBase<Mark>
    {
        [Required,Display(Name ="Mark Type")]
        public MarkType MarkType { get; set; }
        [Required]
        public byte Value { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public string GroupCourseId { get; set; }
    }
}
