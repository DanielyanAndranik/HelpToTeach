using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAll();
        Task<Group> Get(string id);
        Task<IEnumerable<Group>> GetByLecturer(string id);
        Task<Group> Create(Group group);
        Task<Group> Update(Group group);
        Task<Group> Upsert(Group group);
        Task Delete(string id);
    }
}
