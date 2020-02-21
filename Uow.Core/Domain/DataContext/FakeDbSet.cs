﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Entities;
using Uow.Core.Infrastructure;

namespace Uow.Core.Domain.DataContext
{
    public abstract class FakeDbSet<TEntity> : DbSet<TEntity>, IDbSet<TEntity> where TEntity : EntityBase, new()
    {
        #region Private Fields
        private readonly ObservableCollection<TEntity> _items;
        private readonly IQueryable _query;
        #endregion Private Fields

        protected FakeDbSet()
        {
            _items = new ObservableCollection<TEntity>();
            _query = _items.AsQueryable();
        }

        IEnumerator IEnumerable.GetEnumerator() { return _items.GetEnumerator(); }
        public IEnumerator<TEntity> GetEnumerator() { return _items.GetEnumerator(); }

        public Expression Expression { get { return _query.Expression; } }

        public Type ElementType { get { return _query.ElementType; } }

        public IQueryProvider Provider { get { return _query.Provider; } }

        public override TEntity Add(TEntity entity)
        {
            _items.Add(entity);
            return entity;
        }

        public override TEntity Remove(TEntity entity)
        {
            _items.Remove(entity);
            return entity;
        }

        public override TEntity Attach(TEntity entity)
        {
            switch (entity.ObjectState)
            {
                case EntityState.Modified:
                    _items.Remove(entity);
                    _items.Add(entity);
                    break;

                case EntityState.Deleted:
                    _items.Remove(entity);
                    break;

                case EntityState.Unchanged:
                case EntityState.Added:
                    _items.Add(entity);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return entity;
        }

        public override TEntity Create() { return new TEntity(); }

        public override TDerivedEntity Create<TDerivedEntity>() { return Activator.CreateInstance<TDerivedEntity>(); }

        public override ObservableCollection<TEntity> Local { get { return _items; } }
    }
}
