using Couchbase.Core;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IBucket bucket;
        public StudentRepository(IHelpToTeachBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }
    
        public async Task<Student> Create(Student student)
        {
            student.Id = Guid.NewGuid().ToString();
            var result = await this.bucket.InsertAsync<Student>($"student::{student.Id}", student);
            return result.Value;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'student'");
            var result = await bucket.QueryAsync<Student>(query);
            return result.Rows;
        }

        public Task<IEnumerable<Student>> GetByLecturer(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Update(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Upsert(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
