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
        Task<Lesson> Get(string id);
        Task<List<Lesson>> GetAll();
        Task<List<Lesson>> GetByLecturer(string id);
        Task<List<Lesson>> GetByGroupCourse(string id);
        Task<Lesson> Update(Lesson lesson);
        Task Delete(string id);
    }
}
