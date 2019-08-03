using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Force.Infrastructure;

namespace Force.Linq
{
    public class Sorter<T>: ISorter<T>
    {
        private readonly string _propertyName;

        private bool _asc;

        public static bool TryParse(string str, out Sorter<T> sorter)
        {
            (string, bool) GetSorting(string prop)
            {
                prop = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(prop);
                var arr = prop.Split(new[]{".", " "}, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 1)
                    return (arr[0], true);
                var sort = arr[1];
                if (string.Equals(sort, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], true);
                if (string.Equals(sort, "DESC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], false);
                
                return (arr[0], true);
            }

            var (name, asc) = GetSorting(str);
            
            if (!Type<T>.PublicProperties.ContainsKey(name))
            {
                sorter = null;
                return false;
            }

            sorter = new Sorter<T>(name, asc);
            return true;
        }
        
        public Sorter(string propertyName, bool asc = true)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            if (!Type<T>.PublicProperties.ContainsKey(_propertyName))
            {
                throw new ArgumentException(propertyName, 
                    $"Property \"{_propertyName}\" doesn't exist in type \"{typeof(T)}\"");
            }

            _asc = asc;
        }

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable)
            => _asc
                ? queryable.OrderBy(_propertyName)
                : queryable.OrderByDescending(_propertyName);
    }
    
    public class Sorter<T, TParam>: ISorter<T>
    {
        private readonly Expression<Func<T, TParam>> _expression;
        private readonly bool _asc;

        public Sorter(Expression<Func<T, TParam>> expression, bool asc = true)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _asc = asc;
        }

        public static implicit operator Expression<Func<T, TParam>>(Sorter<T, TParam> sorter)
            => sorter._expression;

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable)
            => _asc
                ? queryable.OrderBy(_expression)
                : queryable.OrderByDescending(_expression);
    }

    public static class SorterExtensions
    {
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> queryable, ISorter<T> sorter)
            => sorter.Sort(queryable);
    }
}