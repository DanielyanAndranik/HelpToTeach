using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.N1QL;
using HelpToTeach.Data.Models;
using HelpToTeach.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public class MarkRepository : IMarkRepository
    {
        private readonly IBucket bucket;
        public MarkRepository(INamedBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
        }

        public async Task<List<Mark>> AddPredictedMarks(IEnumerable<Mark> marks, string lessonId)
        {
            var returningMarks = new List<Mark>();

            foreach (var mark in marks)
            {
                mark.IsPredicted = true;
                mark.LessonId = lessonId;
                returningMarks.Add(await this.Create(mark));
            }
            return returningMarks;
        }

        public async Task<List<Mark>> AddRange(IEnumerable<Mark> marks)
        {
            var returningMarks = new List<Mark>();
            foreach(var mark in marks)
            {
                returningMarks.Add(await this.Create(mark));
            }
            return returningMarks;
        }

        public async Task<Mark> Create(Mark mark)
        {
            mark.Id = Guid.NewGuid().ToString();
            mark.Created = DateTime.Now;
            mark.Updated = DateTime.Now;
            var result = await this.bucket.InsertAsync<Mark>($"mark::{mark.Id}", mark);
            return result.Value;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task DeletePredictedMarksByLesson(string lessonId)
        {
            var query = new QueryRequest("DELETE FROM HelpToTeachBucket m WHERE m.type = 'mark' AND m.isPredicted = true AND m.lessonid = $lessonId");
            query.AddNamedParameter("$lessonId", lessonId);
            var result = await bucket.QueryAsync<Mark>(query);
        }

        public Task<Mark> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Mark>> GetAll()
        {
            var query = new QueryRequest("SELECT HelpToTeachBucket.* FROM HelpToTeachBucket WHERE type = 'mark'");
            var result = await bucket.QueryAsync<Mark>(query);
            return result.Rows;
        }

        public async Task<List<Mark>> GetMarksByCourse(string id)
        {
            var query = new QueryRequest(
                "SELECT m.* FROM HelpToTeachBucket m " +
                "JOIN HelpToTeachBucket gc ON m.groupCourseId = gc.id " +
                "WHERE m.type = 'mark' AND gc.type = 'groupcourse' " +
                "AND gc.courseId = $courseId");
            query.AddNamedParameter("$courseId", id);
            var result = await bucket.QueryAsync<Mark>(query);
            return result.Rows;
        }

        public async Task<List<Mark>> GetMarksByStudent(string id)
        {
            var query = new QueryRequest(
                "SELECT m.* FROM HelpToTeachBucket m " +
                "WHERE m.type = 'mark' AND m.studentId = $studentId");
            query.AddNamedParameter("$studentId", id);
            var result = await bucket.QueryAsync<Mark>(query);
            return result.Rows;
        }

        public async Task<List<Mark>> GetMarksByStudentAndCourse(string studentId, string courseId)
        {
            var query = new QueryRequest(
                "SELECT m.* FROM HelpToTeachBucket m " +
                "JOIN HelpToTeachBucket gc ON m.groupCourseId = gc.id " +
                "WHERE m.type = 'mark' AND gc.type = 'groupcourse' " +
                "AND gc.courseId = $courseId AND m.studentId = $studentId");
            query.AddNamedParameter("$studentId", studentId);
            query.AddNamedParameter("$courseId", courseId);
            var result = await bucket.QueryAsync<Mark>(query);
            return result.Rows;
        }

        public async Task<List<Mark>> GetMarksByStudentAndGroupCourse(string studentId, string groupCourseId)
        {
            var query = new QueryRequest(
                "SELECT m.*, l as `lesson` FROM HelpToTeachBucket m " +
                "JOIN HelpToTeachBucket l ON m.lessonId = l.id " +
                "WHERE m.type = 'mark' AND l.type = 'lesson' " +
                "AND l.groupCourseId = $groupCourseId AND m.studentId = $studentId");
            query.AddNamedParameter("$groupCourseId", groupCourseId);
            query.AddNamedParameter("$studentId", studentId);
            var result = await bucket.QueryAsync<Mark>(query);
            return result.Rows;
        }

        public async Task<List<Mark>> GetPredictedMarksByLesson(string lessonId, int type)
        {
            var query = new QueryRequest(
                "SELECT m.*, l as `lesson` FROM HelpToTeachBucket m " +
                "JOIN HelpToTeachBucket l ON m.lessonId = l.id " +
                "WHERE m.type = 'mark' AND l.type = 'lesson' " +
                "AND l.id = $lessonId AND l.lessontype = $type");
            query.AddNamedParameter("$lessonId", lessonId);
            query.AddNamedParameter("$type", type);
            var result = await bucket.QueryAsync<Mark>(query);
            return result.Rows;
        }

        public Task<Mark> Update(Mark mark)
        {
            throw new NotImplementedException();
        }

        public Task<Mark> Upsert(Mark mark)
        {
            throw new NotImplementedException();
        }
    }
}
