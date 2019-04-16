using HelpToTeach.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpToTeach.Data.Models
{
    class Mark : EntityBase<Mark>
    {
        public MarkType MarkType { get; set; }
        public byte Value { get; set; }
        public DateTime Date { get; set; }
        public string StudentId { get; set; }
        public string GroupCourseId { get; set; }
    }
}
