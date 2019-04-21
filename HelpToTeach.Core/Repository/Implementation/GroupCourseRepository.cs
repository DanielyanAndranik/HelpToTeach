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
            return (await this.bucket.GetDocumentAsync<GroupCourse>($"groupCourse::{id}")).Content;
        }

        public async Task<List<GroupCourse>> GetByLecturer(string id) {
            var query = new QueryRequest(
                    "SELECT gc.*, c as `course`, g as `group`, l as `lecturer` " +
                    "FROM HelpToTeachBucket gc " +
                    "JOIN HelpToTeachBucket g ON gc.groupId = g.id " +
                    "JOIN HelpToTeachBucket c ON gc.courseId = c.id " +
                    "JOIN HelpToTeachBucket l ON gc.userId = l.id " +
                    "WHERE c.type = 'course' AND g.type = 'group' " +
                    "AND gc.type = 'groupcourse' AND l.type='user' AND gc.userId = $userId"
                );
            query.AddNamedParameter("$userId", id);
            var result = await bucket.QueryAsync<GroupCourse>(query);
            return result.Rows;
        }

        public async Task<List<GroupCourse>> GetAll()
        {
            var query = new QueryRequest(
                    "SELECT gc.*, c as `course`, g as `group`, l as `lecturer` " +
                    "FROM HelpToTeachBucket gc " +
                    "JOIN HelpToTeachBucket g ON gc.groupId = g.id " +
                    "JOIN HelpToTeachBucket c ON gc.courseId = c.id " +
                    "JOIN HelpToTeachBucket l ON gc.userId = l.id " +
                    "WHERE c.type = 'course' AND g.type = 'group' " +
                    "AND gc.type = 'groupcourse' AND l.type='user'");
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
