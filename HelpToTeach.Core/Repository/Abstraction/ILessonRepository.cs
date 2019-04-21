using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface ILessonRepository
    {
        Task<Lesson> Create(Lesson lesson);
        Task<List<Lesson>> GetByLecturer(string id);
        Task<List<Lesson>> GetByGroupCourse(string id);
    }
}
