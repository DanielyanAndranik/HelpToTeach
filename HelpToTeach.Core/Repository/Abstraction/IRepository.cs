﻿using HelpToTeach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpToTeach.Core.Repository
{
    public interface IRepository<T> where T : EntityBase<T>
    {
        Task<List<T>> GetAll(Type type);
        Task<T> Get(string id);
        Task<T> Create(T item);
        Task<T> Update(T item);
        Task<T> Upsert(T item);
        Task Delete(string id);
    }
}
