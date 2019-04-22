using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using HelpToTeach.Data.Enums;
using HelpToTeach.Data.Models;

namespace HelpToTeach.Core.Repository
{
    public class MLDataRepository : IMLDataRepository
    {
        private readonly IBucket bucket;
        private readonly IGroupCourseRepository groupCourseRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMarkRepository markRepository;

        public MLDataRepository
            (
            INamedBucketProvider provider, 
            IGroupCourseRepository groupCourseRepository, 
            IStudentRepository studentRepository,
            ILessonRepository lessonRepository,
            IMarkRepository markRepository
            )
        {
            this.bucket = provider.GetBucket();
            this.groupCourseRepository = groupCourseRepository;
            this.studentRepository = studentRepository;
            this.lessonRepository = lessonRepository;
            this.markRepository = markRepository;
        }

        public Task<List<MiddleMarkData>> GetDataForFinal(string groupCourseId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MiddleMarkData>> GetDataForFirstMiddle(string groupCourseId)
        {
            var groupCourse = await groupCourseRepository.Get(groupCourseId);
            var lessons = await lessonRepository.GetByGroupCourse(groupCourseId);
            var students = await studentRepository.GetByGroupId(groupCourse.GroupId);
            var middleMarkDataList = new List<MiddleMarkData>();

            foreach(var student in students)
            {
                var middleMarkData = new MiddleMarkData
                {
                    StudentId = student.Id,
                    HasSchoolarship = student.FullSchoolarship
                };
                var marks = await markRepository.GetMarksByStudentAndGroupCourse(student.Id, groupCourseId);

                var firstMiddleMark = marks.FirstOrDefault(m => m.Lesson.LessonType == LessonType.FirstMiddle);
                if(firstMiddleMark != null)
                {
                    middleMarkData.Mark = !firstMiddleMark.Absent ? firstMiddleMark.Value : 0;
                    marks = marks.Where(m => m.Date < firstMiddleMark.Date).ToList();
                }

                var labMarks = marks.Where(m => m.Lesson.LessonType == LessonType.Lab);
                var labsCount = labMarks.Count();
                middleMarkData.LabsCount = labsCount;
                if (labsCount > 0)
                {
                    middleMarkData.LabAbsenceCount = labMarks.Where(m => m.Absent).Count();
                    middleMarkData.LabMarkCount = labMarks.Where(m => !m.Absent).Count();
                    if (middleMarkData.LabMarkCount > 0)
                        middleMarkData.LabMark = (float)labMarks.Where(m => !m.Absent && m.Value != 0).Sum(m => m.Value) / middleMarkData.LabMarkCount;
                }

                var seminarMarks = marks.Where(m => m.Lesson.LessonType == LessonType.Seminar);
                var seminarsCount = seminarMarks.Count();
                middleMarkData.SeminarsCount = seminarsCount;
                if (seminarsCount > 0)
                {
                    middleMarkData.SeminarAbsenceCount = seminarMarks.Where(m => m.Absent).Count();
                    middleMarkData.SeminarMarkCount = seminarMarks.Where(m => !m.Absent).Count();
                    if (middleMarkData.SeminarMarkCount > 0)
                        middleMarkData.SeminarMark = (float)seminarMarks.Where(m => !m.Absent && m.Value != 0).Sum(m => m.Value) / middleMarkData.SeminarMarkCount;

                }

                var lectureMarks = marks.Where(m => m.Lesson.LessonType == LessonType.Lecture);
                var lecturesCount = lectureMarks.Count();
                middleMarkData.LecturesCount = lecturesCount;
                if (lecturesCount > 0)
                {
                    middleMarkData.LectureAbsenceCount = lectureMarks.Where(m => m.Absent).Count();
                    middleMarkData.LectureMarkCount = lectureMarks.Where(m => !m.Absent).Count();
                    if (middleMarkData.LectureMarkCount > 0)
                        middleMarkData.LectureMark = (float)lectureMarks.Where(m => !m.Absent && m.Value != 0).Sum(m => m.Value) / middleMarkData.LectureMarkCount;

                }
                middleMarkDataList.Add(middleMarkData);
            }

            return middleMarkDataList;
        }

        public Task<List<MiddleMarkData>> GetDataForSecondMiddle(string groupCourseId)
        {
            throw new NotImplementedException();
        }
    }
}
