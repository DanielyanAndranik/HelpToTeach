using Couchbase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IBucket bucket;
        public GroupRepository(IHelpToTeachBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }

        public async Task<Group> Create(Group group)
        {
            var key = $"group::{Guid.NewGuid().ToString()}";
            var result = await this.bucket.InsertAsync<Group>(key, group);
            return result.Value;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Group> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Group>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Group>> GetByLecturer(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Group> Update(Group group)
        {
            throw new NotImplementedException();
        }

        public Task<Group> Upsert(Group group)
        {
            throw new NotImplementedException();
        }
    }
}
