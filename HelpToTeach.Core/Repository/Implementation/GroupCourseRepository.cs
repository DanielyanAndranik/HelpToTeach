using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.N1QL;
using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public class GroupCourseRepository : IGroupCourseRepository
    {

        private readonly IBucket bucket;
        public GroupCourseRepository(INamedBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }

        public async Task<GroupCourse> Create(GroupCourse groupCourse)
        {
            groupCourse.Id = Guid.NewGuid().ToString();
            groupCourse.Created = DateTime.Now;
            groupCourse.Updated = DateTime.Now;
            var key = $"groupCourse::{groupCourse.Id}";
            var result = await this.bucket.InsertAsync<GroupCourse>(key, groupCourse);
            return result.Value;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<GroupCourse> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GroupCourse>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'groupcourse'");
            var result = await bucket.QueryAsync<GroupCourse>(query);
            return result.Rows;
        }

        public Task<GroupCourse> Update(GroupCourse groupCourse)
        {
            throw new NotImplementedException();
        }

        public Task<GroupCourse> Upsert(GroupCourse groupCourse)
        {
            throw new NotImplementedException();
        }
    }
}
