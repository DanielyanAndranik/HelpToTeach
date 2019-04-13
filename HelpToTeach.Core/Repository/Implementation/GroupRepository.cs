using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.N1QL;
using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IBucket bucket;
        public GroupRepository(INamedBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }

        public async Task<Group> Create(Group group)
        {
            group.Id = Guid.NewGuid().ToString();
            group.Created = DateTime.Now;
            group.Updated = DateTime.Now;
            var result = await this.bucket.InsertAsync<Group>($"group::{group.Id}", group);
            return result.Value;
        }

        public async Task Delete(string id)
        {
            await this.bucket.RemoveAsync($"group::{id}");
        }

        public async Task<Group> Get(string id)
        {
            List<Group> groups = await GetAll();
            Group result = groups.FirstOrDefault(u => u.Id == id);
            return result;
        }

        public async Task<List<Group>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'group'");
            var result = await bucket.QueryAsync<Group>(query);
            return result.Rows;
        }

        public async Task<List<Group>> GetByLecturer(string id)
        {
            return new List<Group>();
        }

        public async Task<Group> Update(Group group)
        {
            group.Updated = DateTime.Now;
            var result = await this.bucket.ReplaceAsync($"group::{group.Id}", group);
            return result.Value;
        }

        public Task<Group> Upsert(Group group)
        {
            throw new NotImplementedException();
        }
    }
}
