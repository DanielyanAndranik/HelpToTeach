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

        public Task<List<Lesson>> GetByLecturer(string id)
        {
            throw new NotImplementedException();
        }
    }
}
