using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Runtasker.Logic.Contexts.Fake
{
    public class FakeDbSet<T> : DbSet<T> where T : class, IEnumerable//,IEnumerator
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

        int index = -1;

        // Реализуем интерфейс IEnumerable
        public IEnumerator GetEnumerator()
        {
            return this as IEnumerator;
        }

        // Реализуем интерфейс IEnumerator
        public bool MoveNext()
        {
            if (index == _data.Count - 1)
            {
                Reset();
                return false;
            }

            index++;
            return true;
        }

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get
            {
                return _data[index];
            }
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
