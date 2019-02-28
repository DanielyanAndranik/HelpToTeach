using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        Task<User> GetUserByAuth0Id(string auth0Id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> AddUser();
        Task<User> EditUser();
        Task<User> DeleteUser();
    }
}