using System;
using System.Linq;
using System.Linq.Expressions;

namespace Force.Linq
{
    public class Sorter<T>: ISorter<T>
    {
        private readonly string _propertyName;

        public Sorter(string propertyName)
        {
            _propertyName = propertyName;
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable)
            => Conventions<T>.Sort(queryable, _propertyName);
    }
    
    public class Sorter<T, TParam>: ISorter<T>
    {
        private readonly Expression<Func<T, TParam>> _expression;

        public Sorter(Expression<Func<T, TParam>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public Sorter(string propertyName)
        {
            throw new NotImplementedException();
        }
        
        public static implicit operator Expression<Func<T, TParam>>(Sorter<T, TParam> sorter)
            => sorter._expression;

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable)
            => queryable.OrderBy(_expression);
    }

    public static class SorterExtensions
    {
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> queryable, ISorter<T> sorter)
            => sorter.Sort(queryable);
    }
}