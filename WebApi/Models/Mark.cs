using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{

    public enum MarkType
    {
        TermMark,
        FinalMark
    }

    public class Mark:EntityBase<Mark>
    {
        public MarkType MarkType { get; set; }
        public int Value { get; set; }
        public string StudentId { get; set; }
        public string GroupCourseId { get; set; }
    }
}
