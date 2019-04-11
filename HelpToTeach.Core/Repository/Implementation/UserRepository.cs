using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.N1QL;
using HelpToTeach.Data.Models;

namespace HelpToTeach.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IBucket _bucket;

        public UserRepository(INamedBucketProvider provider)
        {
            this._bucket = provider.GetBucket();
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'user'");
            var result = await _bucket.QueryAsync<User>(query);

            // check if username exists
            if (!result.Success)
                return null;

            var user = result.FirstOrDefault(u => u.Username == username);

            if (user == null) return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            user.Created = DateTime.Now;
            user.Updated = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            var key = CreateKey(typeof(User), user.Id);

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var result = await _bucket.InsertAsync(key, user);
            if (!result.Success) throw result.Exception;

            return result.Value;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(string id)
        {
            var result = await this._bucket.GetDocumentAsync<User>(id);
            return result.Content;
        }

        public async Task<User> GetLecturerById(string id)
        {
            List<User> lecturers = await GetLecturers();
            User result = lecturers.FirstOrDefault(t => t.Id == id);
            return result;
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetLecturers()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'user'");
            var users = await _bucket.QueryAsync<User>(query);
            var result = (from s in users where s.Role == "1" select s).ToList();
            return result;
        }

        public Task<User> Update(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> Upsert(User user)
        {
            throw new NotImplementedException();
        }

        private string CreateKey(Type t, string id)
        {
            // generates type-prefixed key like 'player::123'
            return string.Format("{0}::{1}", t.Name.ToLower(), id);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
