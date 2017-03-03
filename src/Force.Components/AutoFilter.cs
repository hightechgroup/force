using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using Force.Cqrs;
using JetBrains.Annotations;

namespace Force.Components
{
    public static class AutoFilterExtensions
    {
        public static IQueryable<T> ApplyDictionary<T>(this IQueryable<T> query, IDictionary<string, object> filters)
            => filters.Aggregate(query, (current, kv) => current.Where(kv.Value is string
                    ? $"{kv.Key}.StartsWith(@0)"
                    : $"{kv.Key}=@0", kv.Value));

        public static IDictionary<string, object> GetFilters(this object o)
            => o.GetType()
            .GetTypeInfo()
            .GetProperties(BindingFlags.Public)
            .Where(x => x.CanRead)
            .ToDictionary(k => k.Name, v => v.GetValue(o));
    }

    public class AutoFilter<T> : ILinqSpecification<T>
        where T: class
    {
        public IDictionary<string, object> Filter { get; } 

        public AutoFilter()
        {
            Filter = new Dictionary<string, object>();
        }

        public AutoFilter([NotNull] IDictionary<string, object> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            Filter = filter;
        }

        public IQueryable<T> Apply(IQueryable<T> query)
            => query.ApplyDictionary(Filter);
    }
}
