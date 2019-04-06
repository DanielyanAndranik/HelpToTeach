using HelpToTeach.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<List<User>> GetAll();
        Task<User> GetTeacherById(string id);
        Task<List<User>> GetTeachers();
        Task<User> Get(string id);
        Task<User> Create(User user, string password);
        Task<User> Update(User user);
        Task<User> Upsert(User user);
        Task Delete(string id);
    }
}