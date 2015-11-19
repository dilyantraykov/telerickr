﻿namespace Telerickr.Services.Tests.TestObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Telerickr.Data;

    public class InMemoryRepository<T> : IRepository<T>
        where T : class
    {
        private IList<T> data;

        public InMemoryRepository()
        {
            this.data = new List<T>();
            this.AttachedEntities = new List<T>();
            this.DetachedEntities = new List<T>();
            this.UpdatedEntities = new List<T>();
        }

        public int NumberOfModels { get { return this.data.Count;} }

        public IList<T> AttachedEntities { get; private set; }

        public IList<T> DetachedEntities { get; private set; }

        public IList<T> UpdatedEntities { get; private set; }

        public bool IsDisposed { get; private set; }

        public int NumberOfSaves { get; private set; }

        public void Add(T entity)
        {
            this.data.Add(entity);
        }

        public IQueryable<T> All()
        {
            return this.data.AsQueryable();
        }

        public T Attach(T entity)
        {
            this.AttachedEntities.Add(entity);
            return entity;
        }

        public void Delete(object id)
        {
            if (this.data.Count == 0)
            {
                throw new InvalidOperationException("Nothing to delete.");
            }

            this.data.RemoveAt(0);
        }

        public void Delete(T entity)
        {
            if (this.data.Count == 0)
            {
                throw new InvalidOperationException("Nothing to delete.");
            }

            this.data.Remove(entity);
        }

        public void Detach(T entity)
        {
            this.DetachedEntities.Add(entity);
        }

        public void Dispose()
        {
            this.IsDisposed = true;
        }

        public T GetById(object id)
        {
            if (this.data.Count == 0)
            {
                throw new InvalidOperationException("Nothing to delete.");
            }

            return this.data[0];
        }

        public int SaveChanges()
        {
            this.NumberOfSaves++;
            return 1;
        }

        public void Update(T entity)
        {
            this.UpdatedEntities.Add(entity);
        }
    }
}
