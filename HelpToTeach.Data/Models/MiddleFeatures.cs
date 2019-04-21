using System;
using System.Collections.Generic;
using System.Text;

namespace HelpToTeach.Data.Models
{
    public class MiddleMarkData
    {
        public string StudentId { get; set; }
        public bool HasSchoolarship { get; set; }
        public float LabMark { get; set; }
        public float LabAbsence { get; set; }
        public float LectureActivity { get; set; }
        public float LectureAbsence { get; set; }
        public float SeminarActivity { get; set; }
        public float SeminarAbsence { get; set; }
        public int Mark { get; set; }
    }
}
