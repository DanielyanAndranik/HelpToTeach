using Couchbase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IBucket bucket;
        public CourseRepository(IHelpToTeachBucketProvider provider)
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

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Course> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Course>> GetByLecturer(string id)
        {
            return null;
        }

        public Task<Course> Update(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<Course> Upsert(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
