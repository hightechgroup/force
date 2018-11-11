using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Force.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Force.AutoMapper
{
    public class Aggregate<T>
        where T : class
    {
        private readonly DbContext _dbContext;
        public T Entity { get; }

        public Aggregate(DbContext dbContext, T entity)
        {
            if (dbContext != null) _dbContext = dbContext;
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public IQueryable<T> Query => _dbContext.Set<T>();

        public Aggregate<TR> Reference<TR>(Expression<Func<T, TR>> selector) 
            where TR : class
        {
            var propertyName = ((MemberExpression) selector.Body).Member.Name + "Id";
            var id = Entity.GetType().GetProperty(propertyName).GetValue(Entity);
            return _dbContext.Find<TR>(id).PipeTo(x => x == null 
                ? null
                : new Aggregate<TR>(_dbContext, x));
        }

        public IQueryable<TR> Collection<TR>(Expression<Func<T, IEnumerable<TR>>> selector)
            where TR : class
            => _dbContext
                .Entry(Entity)
                .Collection(selector)
                .Query();
    }
}