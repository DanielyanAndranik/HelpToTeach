using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpToTeach.Data.Transfer
{
    public class StudentMark
    {
        public string StudentId { get; set; }
        public byte Mark { get; set; }
        public bool Absent { get; set; }

        public static implicit operator Mark(StudentMark studentMark)
        {
            return new Mark
            {
                Absent = studentMark.Absent,
                StudentId = studentMark.StudentId,
                Value = studentMark.Mark
            };
        }
    }
}
