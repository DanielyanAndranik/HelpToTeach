using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Group>> GetAll();
        Task<Group> Get(string id);
        Task<IEnumerable<Group>> GetByLecturer(string id);
        Task<Group> Create(Group course);
        Task<Group> Update(Group course);
        Task<Group> Upsert(Group course);
        Task Delete(string id);
    }
}
