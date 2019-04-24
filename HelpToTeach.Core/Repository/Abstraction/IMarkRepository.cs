using HelpToTeach.Data.Models;
using HelpToTeach.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IMarkRepository
    {
        Task<List<Mark>> AddRange(IEnumerable<Mark> marks);
        Task<List<Mark>> AddPredictedMarks(IEnumerable<Mark> marks, string lessonId);
        Task<List<Mark>> GetAll();
        Task<Mark> Get(string id);
        Task<Mark> Create(Mark mark);
        Task<Mark> Update(Mark mark);
        Task<Mark> Upsert(Mark mark);
        Task Delete(string id);
        Task DeletePredictedMarksByLesson(string lessonId);
        Task<List<Mark>> GetMarksByStudent(string id);
        Task<List<Mark>> GetMarksByCourse(string id);
        Task<List<Mark>> GetMarksByStudentAndCourse(string studentId, string courseId);
        Task<List<Mark>> GetMarksByStudentAndGroupCourse(string studentId, string groupCourseId);
        Task<List<Mark>> GetPredictedMarksByLesson(string lessonId, int type);
    }
}
