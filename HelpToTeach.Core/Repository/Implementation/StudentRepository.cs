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
    public class StudentRepository : IStudentRepository
    {
        private readonly IBucket bucket;
        private readonly IGroupRepository groupRepository;
        public StudentRepository(INamedBucketProvider provider, IGroupRepository groupRepository)
        {
            this.bucket = provider.GetBucket();
            this.groupRepository = groupRepository;
        }
    
        public async Task<Student> Create(Student student)
        {
            student.Id = Guid.NewGuid().ToString();
            var result = await this.bucket.InsertAsync<Student>($"student::{student.Id}", student);
            return result.Value;
        }

        public async Task Delete(string id)
        {
            await this.bucket.RemoveAsync($"student::{id}");
        }

        public async Task<Student> Get(string id)
        {
            return (await this.bucket.GetAsync<Student>($"student::{id}")).Value;
        }

        public async Task<List<Student>> GetAll()
        {
            var query = new QueryRequest(
                "SELECT s.*, g as `group` FROM HelpToTeachBucket s " +
                "JOIN HelpToTeachBucket g ON s.groupId = g.id " +
                "WHERE s.type = 'student' AND g.type = 'group'");
            var result = await bucket.QueryAsync<Student>(query);
            return result.Rows;
        }

        public async Task<List<Student>> GetByGroupId(string id)
        {
            var query = new QueryRequest(
                "SELECT s.*, g as `group` FROM HelpToTeachBucket s " +
                "JOIN HelpToTeachBucket g ON s.groupId = g.id " +
                "WHERE s.type = 'student' AND g.type = 'group'");
            var result = await bucket.QueryAsync<Student>(query);
            return result.Rows;
        }

        public async Task<List<Student>> GetByLecturer(string id)
        {

            //var query = new QueryRequest(
            //                                "SELECT s.*, g as `group` FROM HelpToTeachBucket s " +
            //                                "JOIN HelpToTeachBucket g ON s.groupId = g.id" +
            //                                "JOIN HelpToTeachBucket gc ON gc.groupId = g.id" +
            //                                "WHERE s.type = 'student' AND g.type = 'group' AND gc.type = 'groupcourse'" +
            //                                "AND gc.userId = '$userId'"
            //                            );
            //query.AddNamedParameter("$userId", id);
            //var result = await bucket.QueryAsync<Student>(query);
            //return result.Rows;

            List<Student> allStudents = await GetAll();
            List<Group> allByLecturer = await groupRepository.GetByLecturer(id);
            List<Student> result = new List<Student>();

            foreach (var group in allByLecturer)
            {
                foreach (var student in allStudents)
                {
                    if (student.GroupId == group.Id) {
                        result.Add(student);
                    }
                }
            }

            return result;
        }

        public async Task<Student> Update(Student student)
        {
            student.Updated = DateTime.Now;
            var result = await this.bucket.ReplaceAsync($"student::{student.Id}", student);
            return result.Value;
        }

        public Task<Student> Upsert(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
