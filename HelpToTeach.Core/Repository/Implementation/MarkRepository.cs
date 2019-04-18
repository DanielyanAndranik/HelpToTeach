using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using HelpToTeach.Core.Repository.Abstraction;
using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository.Implementation
{
    public class MarkRepository : IMarkRepository
    {

        private readonly IBucket bucket;
        public MarkRepository(INamedBucketProvider provider)
        {
            this.bucket = provider.GetBucket();
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

        public Task<Mark> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Mark>> GetAll()
        {
            throw new NotImplementedException();
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
