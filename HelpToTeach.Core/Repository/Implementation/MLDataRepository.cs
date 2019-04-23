using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using HelpToTeach.Core.AI;
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

        public async Task<List<FinalMarkFeatures>> GetDataForFinal(string groupCourseId)
        {
            var groupCourses = new List<GroupCourse>();
            if (groupCourseId == null)
            {
                groupCourses.AddRange(await groupCourseRepository.GetAll());
            }
            else
            {
                groupCourses.Add(await groupCourseRepository.Get(groupCourseId));
            }
            var finalMarkDataList = new List<FinalMarkFeatures>();

            foreach (var groupCourse in groupCourses)
            {
                var lessons = await lessonRepository.GetByGroupCourse(groupCourse.Id);
                var students = await studentRepository.GetByGroupId(groupCourse.GroupId);

                foreach (var student in students)
                {
                    var finalMarkData = new FinalMarkFeatures
                    {
                        StudentId = student.Id,
                        HasScholarship = student.FullScholarship
                    };

                    var marks = await markRepository.GetMarksByStudentAndGroupCourse(student.Id, groupCourse.Id);


                    var firstMiddleMark = marks.FirstOrDefault(m => m.Lesson.LessonType == LessonType.FirstMiddle);
                    if (firstMiddleMark != null)
                    {
                        finalMarkData.FirstMiddleMark = !firstMiddleMark.Absent ? firstMiddleMark.Value : 0;
                    }

                    var secondMiddleMark = marks.FirstOrDefault(m => m.Lesson.LessonType == LessonType.SecondMiddle);
                    if (secondMiddleMark != null)
                    {
                        finalMarkData.SecondMiddleMark = !secondMiddleMark.Absent ? secondMiddleMark.Value : 0;
                    }

                    var finalMark = marks.FirstOrDefault(m => m.Lesson.LessonType == LessonType.Final);
                    if(finalMark != null)
                    {
                        finalMarkData.Mark = !finalMark.Absent ? finalMark.Value : 0;
                    }

                    var labMarks = marks.Where(m => m.Lesson.LessonType == LessonType.Lab);
                    var labsCount = labMarks.Count();
                    finalMarkData.LabsCount = labsCount;
                    if (labsCount > 0)
                    {
                        finalMarkData.LabAbsenceCount = labMarks.Where(m => m.Absent).Count();
                        finalMarkData.LabMarkCount = labMarks.Where(m => !m.Absent).Count();
                        if (finalMarkData.LabMarkCount > 0)
                            finalMarkData.LabMark = (float)labMarks.Where(m => !m.Absent && m.Value != 0).Sum(m => m.Value) / finalMarkData.LabMarkCount;
                    }

                    var seminarMarks = marks.Where(m => m.Lesson.LessonType == LessonType.Seminar);
                    var seminarsCount = seminarMarks.Count();
                    finalMarkData.SeminarsCount = seminarsCount;
                    if (seminarsCount > 0)
                    {
                        finalMarkData.SeminarAbsenceCount = seminarMarks.Where(m => m.Absent).Count();
                        finalMarkData.SeminarMarkCount = seminarMarks.Where(m => !m.Absent).Count();
                        if (finalMarkData.SeminarMarkCount > 0)
                            finalMarkData.SeminarMark = (float)seminarMarks.Where(m => !m.Absent && m.Value != 0).Sum(m => m.Value) / finalMarkData.SeminarMarkCount;

                    }

                    var lectureMarks = marks.Where(m => m.Lesson.LessonType == LessonType.Lecture);
                    var lecturesCount = lectureMarks.Count();
                    finalMarkData.LecturesCount = lecturesCount;
                    if (lecturesCount > 0)
                    {
                        finalMarkData.LectureAbsenceCount = lectureMarks.Where(m => m.Absent).Count();
                        finalMarkData.LectureMarkCount = lectureMarks.Where(m => !m.Absent).Count();
                        if (finalMarkData.LectureMarkCount > 0)
                            finalMarkData.LectureMark = (float)lectureMarks.Where(m => !m.Absent && m.Value != 0).Sum(m => m.Value) / finalMarkData.LectureMarkCount;

                    }
                    finalMarkDataList.Add(finalMarkData);
                }
            }

            return finalMarkDataList;
        }

        public async Task<List<MiddleMarkFeatures>> GetDataForFirstMiddle(string groupCourseId)
        {
            var groupCourse = await groupCourseRepository.Get(groupCourseId);
            var lessons = await lessonRepository.GetByGroupCourse(groupCourseId);
            var students = await studentRepository.GetByGroupId(groupCourse.GroupId);
            var middleMarkDataList = new List<MiddleMarkFeatures>();

            foreach (var student in students)
            {
                var middleMarkData = new MiddleMarkFeatures
                {
                    StudentId = student.Id,
                    HasScholarship = student.FullScholarship
                };
                var marks = await markRepository.GetMarksByStudentAndGroupCourse(student.Id, groupCourseId);

                var firstMiddleMark = marks.FirstOrDefault(m => m.Lesson.LessonType == LessonType.FirstMiddle);
                if (firstMiddleMark != null)
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

        public Task<List<MiddleMarkFeatures>> GetDataForSecondMiddle(string groupCourseId)
        {
            throw new NotImplementedException();
        }

        public Task<KeyValuePair<bool, List<Mark>>> GetFinalPrediction(string groupCourseId)
        {
            throw new NotImplementedException();
        }

        public async Task<KeyValuePair<bool, List<Mark>>> GetFirstMiddlePrediction(string groupCourseId)
        {
            var result = await Task.Run<bool>(() => PythonRunner.Run(OperationNames.PredictForFirstMiddle, groupCourseId));
            if (!result)
                return new KeyValuePair<bool, List<Mark>>(false, null);

            var firstMiddle = (await lessonRepository.GetByGroupCourse(groupCourseId)).FirstOrDefault(l => l.LessonType == LessonType.FirstMiddle);

            var predictedMiddleMarks = await markRepository.GetPredictedMarksByLesson(firstMiddle.Id, (int)firstMiddle.LessonType);

            return new KeyValuePair<bool, List<Mark>>(true, predictedMiddleMarks);
        }

        public async Task<KeyValuePair<bool, List<Mark>>> GetSecondMiddlePrediction(string groupCourseId)
        {
            var result = await Task.Run<bool>(() => PythonRunner.Run(OperationNames.PredictForSecondMiddle, groupCourseId));
            if (!result)
                return new KeyValuePair<bool, List<Mark>>(false, null);

            var secondMiddle = (await lessonRepository.GetByGroupCourse(groupCourseId)).FirstOrDefault(l => l.LessonType == LessonType.SecondMiddle);

            var predictedMiddleMarks = await markRepository.GetPredictedMarksByLesson(secondMiddle.Id, (int)secondMiddle.LessonType);

            return new KeyValuePair<bool, List<Mark>>(true, predictedMiddleMarks);
        }
    }
}
