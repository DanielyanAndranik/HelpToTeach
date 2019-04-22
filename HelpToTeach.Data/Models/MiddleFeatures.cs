using System;
using System.Collections.Generic;
using System.Text;

namespace HelpToTeach.Data.Models
{
    public class MiddleMarkData
    {
        public string StudentId { get; set; }
        public bool HasSchoolarship { get; set; }

        public int LabsCount { get; set; }
        public int LabAbsenceCount { get; set; }
        public int LabMarkCount { get; set; }
        public float LabMark { get; set; }

        public int LecturesCount { get; set; }
        public int LectureAbsenceCount { get; set; }
        public int LectureMarkCount { get; set; }
        public float LectureMark { get; set; }

        public int SeminarsCount { get; set; }
        public int SeminarAbsenceCount { get; set; }
        public int SeminarMarkCount { get; set; }
        public float SeminarMark { get; set; }

        public int Mark { get; set; }
    }
}
