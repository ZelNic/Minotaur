﻿using System.Linq.Expressions;

namespace BookStore.DataBase.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
       
    }
}
