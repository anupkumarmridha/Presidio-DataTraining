﻿namespace WebApplication1.Interfaces
{
     public interface IRepository<K, T> where T : class
    {
        public Task<T> Add(T item);
        public IQueryable<T> Query();
        public Task<T> Delete(K key);
        public Task<T> Update(T item);
        public Task<T> Get(K key);
     
        public Task<IEnumerable<T>> Get();
    }
}

