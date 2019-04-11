using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IGroupCourseRepository
    {
        Task<List<GroupCourse>> GetAll();
        Task<GroupCourse> Get(string id);
        Task<List<GroupCourse>> GetByLecturerId(string id);
        Task<GroupCourse> Create(GroupCourse groupCourse);
        Task<GroupCourse> Update(GroupCourse groupCourse);
        Task<GroupCourse> Upsert(GroupCourse groupCourse);
        Task Delete(string id);
    }
}
