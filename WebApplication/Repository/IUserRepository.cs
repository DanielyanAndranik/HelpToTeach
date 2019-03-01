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
        Task<User> AddUser(User user);
        Task<User> EditUser(User user);
        Task<User> DeleteUser(int id);
    }
}