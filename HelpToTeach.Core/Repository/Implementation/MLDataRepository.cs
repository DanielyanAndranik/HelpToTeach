using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using HelpToTeach.Data.Models;

namespace HelpToTeach.Core.Repository
{
    public class MLDataRepository : IMLDataRepository
    {
        private readonly IBucket bucket;
        private readonly IGroupCourseRepository groupCourseRepository;
        private readonly IStudentRepository studentRepository;

        public MLDataRepository
            (
            INamedBucketProvider provider, 
            IGroupCourseRepository groupCourseRepository, 
            IStudentRepository studentRepository
            )
        {
            this.bucket = provider.GetBucket();
            this.groupCourseRepository = groupCourseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<List<MiddleFeatures>> GetDataForFinal(string groupCourseId)
        {
            var groupCourse = await groupCourseRepository.Get(groupCourseId);
            //var lessons = await 
            var students = studentRepository.GetByGroupId(groupCourse.GroupId);
            var middleFeatures = new List<MiddleFeatures>();

            return middleFeatures;
        }

        public Task<List<MiddleFeatures>> GetDataForFirstMiddle(string groupCourseId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MiddleFeatures>> GetDataForSecondMiddle(string groupCourseId)
        {
            throw new NotImplementedException();
        }
    }
}
