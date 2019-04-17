using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository.Abstraction
{
    public interface IMarkRepository
    {
        Task<List<Mark>> GetAll();
        Task<Mark> Get(string id);
        Task<Mark> Create(Mark mark);
        Task<Mark> Update(Mark mark);
        Task<Mark> Upsert(Mark mark);
        Task Delete(string id);
    }
}
