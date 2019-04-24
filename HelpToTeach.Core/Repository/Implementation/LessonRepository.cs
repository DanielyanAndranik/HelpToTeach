using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.N1QL;
using HelpToTeach.Data.Models;

namespace HelpToTeach.Core.Repository
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IBucket bucket;
        public LessonRepository(INamedBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }

        public async Task<Lesson> Create(Lesson lesson)
        {
            lesson.Id = Guid.NewGuid().ToString();
            lesson.Created = DateTime.Now;
            lesson.Updated = DateTime.Now;
            await this.bucket.InsertAsync<Lesson>($"lesson::{lesson.Id}", lesson);
            var result = (await bucket.GetDocumentAsync<Lesson>($"lesson::{lesson.Id}")).Content;
            return result;
        }

        public async Task Delete(string id)
        {
            await this.bucket.RemoveAsync($"group::{id}");
        }

        public async Task<Lesson> Get(string id)
        {
            return (await bucket.GetDocumentAsync<Lesson>(id)).Content;
        }

        public async Task<List<Lesson>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'lesson'");
            var result = await bucket.QueryAsync<Lesson>(query);
            return result.Rows;
        }

        public async Task<List<Lesson>> GetByGroupCourse(string id)
        {
            var query = new QueryRequest(
                    "SELECT l.* FROM HelpToTeachBucket l " +
                    "WHERE l.type = 'lesson' AND l.groupCourseId = $groupCourseId"
                );
            query.AddNamedParameter("$groupCourseId", id);
            var result = await bucket.QueryAsync<Lesson>(query);
            return result.Rows;
        }

        public async Task<List<Lesson>> GetByLecturer(string id)
        {
            var query = new QueryRequest(
                "SELECT l.*, gc as `groupCourse` FROM HelpToTeachBucket l " +
                "JOIN HelpToTeachBucket gc ON l.groupCourseId = gc.id " +
                "WHERE l.type = 'lesson' AND gc.type ='groupcourse' AND gc.userId = $lecturerId"
            );
            query.AddNamedParameter("$lecturerId", id);
            var result = await bucket.QueryAsync<Lesson>(query);
            return result.Rows;
        }

        public async Task<Lesson> Update(Lesson lesson)
        {
            lesson.Updated = DateTime.Now;
            var result = await this.bucket.ReplaceAsync($"lesson::{lesson.Id}", lesson);
            return result.Value;
        }
    }
}
