using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAll();
        Task<Group> Get(string id);
        Task<List<Group>> GetByLecturer(string id);
        Task<Group> Create(Group group);
        Task<Group> Update(Group group);
        Task<Group> Upsert(Group group);
        Task Delete(string id);
    }
}
