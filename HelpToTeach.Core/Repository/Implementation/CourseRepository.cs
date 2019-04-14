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
    public class CourseRepository : ICourseRepository
    {
        private readonly IBucket bucket;
        public CourseRepository(INamedBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }
        public async Task<Course> Create(Course course)
        {
            course.Id = Guid.NewGuid().ToString();
            var key = $"course::{course.Id}";
            var result = await this.bucket.InsertAsync<Course>(key, course);
            return result.Value;
        }

        public async Task Delete(string id)
        {
            await this.bucket.RemoveAsync($"course::{id}");
        }

        public async Task<Course> Get(string id)
        {
            var courses = await GetAll();
            Course course = courses.FirstOrDefault(u => u.Id == id);
            return course;
        }

        public async Task<List<Course>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'course'");
            var result = await bucket.QueryAsync<Course>(query);
            return result.Rows;
        }

        public async Task<List<Course>> GetByLecturer(string id)
        {
            return new List<Course>();
        }

        public async Task<Course> Update(Course course)
        {
            course.Updated = DateTime.Now;
            var result = await this.bucket.ReplaceAsync($"course::{course.Id}", course);
            return result.Value;
        }

        public Task<Course> Upsert(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
