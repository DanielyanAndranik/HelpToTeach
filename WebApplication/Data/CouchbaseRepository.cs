using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using HelpToTeach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Data
{
    public class CouchbaseRepository<T> : IRepository<T> where T : EntityBase<T>
    {
        private readonly IBucket _bucket = ClusterHelper.GetBucket("HelpToTeachMainBucket");

        #region Private Methods

        private string CreateKey(string id)
        {
            // generates type-prefixed key like 'player::123'
            return string.Format("{0}::{1}", typeof(T).Name.ToLower(), id);
        }

        #endregion

        #region Public Members

        #region CRUD

        public async Task<List<T>> GetAll(Type t)
        {
            var type = t.Name.ToLower();
            var query = new QueryRequest("SELECT HelpToTeachMainBucket.* FROM HelpToTeachMainBucket WHERE type = $type");
            query.AddNamedParameter("type", type);
            var result = await _bucket.QueryAsync<T>(query);
            return !result.Success ? null : result.Rows;
        }

        public async Task<T> Get(string id)
        {
            var key = CreateKey(id);
            var result = await _bucket.GetAsync<T>(key);
            return !result.Success ? null : result.Value;
        }

        public async Task<T> Create(T item)
        {
            item.Created = DateTime.Now;
            item.Updated = DateTime.Now;
            item.Id = Guid.NewGuid().ToString();
            var key = CreateKey(item.Id);

            var result = await _bucket.InsertAsync(key, item);
            if (!result.Success) throw result.Exception;

            return item;
        }

        public async Task<T> Update(T item)
        {
            item.Updated = DateTime.Now;
            var key = CreateKey(item.Id);
            var result = await _bucket.ReplaceAsync(key, item);

            if (!result.Success) throw result.Exception;

            return item;
        }

        public async Task<T> Upsert(T item)
        {
            if (Get(item.Id) == null) item.Created = DateTime.Now;
            item.Updated = DateTime.Now;
            var key = CreateKey(item.Id);
            var result = await _bucket.UpsertAsync(key, item);

            if (!result.Success) throw result.Exception;

            return item;
        }

        public async Task Delete(string id)
        {
            var key = CreateKey(id);
            var result = await _bucket.RemoveAsync(key);
            if (!result.Success) throw result.Exception;
        }

        #endregion

        #endregion
    }
}
