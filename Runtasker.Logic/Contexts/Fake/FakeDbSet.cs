using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Runtasker.Logic.Contexts.Fake
{
    /// <summary>
    /// This is an in-memory, List backed implementation of
    /// Entity Framework's System.Data.Entity.IDbSet to use
    /// for testing.
    /// </summary>
    /// <typeparam name="T">The type of entity to store.</typeparam>
    public class FakeDbSet<T> : DbSet<T>, IDbSet<T> where T : class
    {
        private readonly List<T> _data;

        public FakeDbSet()
        {
            _data = new List<T>();
        }

        public FakeDbSet(params T[] entities)
        {
            _data = new List<T>(entities);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Expression Expression
        {
            get { return Expression.Constant(_data.AsQueryable()); }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public IQueryProvider Provider
        {
            get { return _data.AsQueryable().Provider; }
        }

        public override T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Wouldn't you rather use Linq .SingleOrDefault()?");
        }

        public override T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public override T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        //public override TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        //{
        //    return Activator.CreateInstance<TDerivedEntity>();
        //}

        public override ObservableCollection<T> Local
        {
            get { return new ObservableCollection<T>(_data); }
        }
    }
}
