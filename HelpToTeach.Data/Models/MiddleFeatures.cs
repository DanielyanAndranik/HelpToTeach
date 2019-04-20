using System;
using System.Collections.Generic;
using System.Text;

namespace HelpToTeach.Data.Models
{
    public class MiddleFeatures
    {
        public string StudentId { get; set; }
        public bool HasSchoolarship { get; set; }
        public float LabMark { get; set; }
        public float LabPresence { get; set; }
        public float LectureActivity { get; set; }
        public float LecturePresece { get; set; }
        public float SeminarActivity { get; set; }
        public float SeminarPresece { get; set; }
    }
}
