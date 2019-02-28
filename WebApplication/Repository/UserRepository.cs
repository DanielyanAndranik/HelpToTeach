using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HelpToTeachContext db;

        public UserRepository(HelpToTeachContext context)
        {
            this.db = context;
        }
        public Task<User> AddUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> EditUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByAuth0Id(string auth0Id)
        {
            return await Task.Factory.StartNew(() => this.db.Users.First(u => u.Auth0Id == auth0Id));
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
