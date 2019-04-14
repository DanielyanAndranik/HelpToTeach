using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.N1QL;
using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task Delete(string id)
        {
            await this.bucket.RemoveAsync($"groupCourse::{id}");
        }

        public async Task<GroupCourse> Get(string id)
        {
            return (await this.bucket.GetAsync<GroupCourse>($"groupCourse::{id}")).Value;
        }

        public async Task<List<GroupCourse>> GetByLecturerId(string id) {
            List<GroupCourse> all = await GetAll();
            List<GroupCourse> result = (from g in all where g.UserId == id select g).ToList();
            return result;
        }

        public async Task<List<GroupCourse>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'groupcourse'");
            var result = await bucket.QueryAsync<GroupCourse>(query);
            return result.Rows;
        }

        public async Task<GroupCourse> Update(GroupCourse groupCourse)
        {
            groupCourse.Updated = DateTime.Now;
            var result = await this.bucket.ReplaceAsync($"groupCourse::{groupCourse.Id}", groupCourse);
            return result.Value;
        }

        public Task<GroupCourse> Upsert(GroupCourse groupCourse)
        {
            throw new NotImplementedException();
        }
    }
}
