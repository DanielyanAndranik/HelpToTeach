using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
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

        public Task<List<Lesson>> GetByLecturer(string id)
        {
            throw new NotImplementedException();
        }
    }
}
