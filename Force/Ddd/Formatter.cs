using System;
using System.Linq.Expressions;
using Force.Expressions;
using Force.Extensions;

namespace Force.Ddd
{
    public class Formatter<T>
    {
        public static implicit operator Formatter<T> (Expression<Func<T, string>> expresssion) 
            => new Formatter<T>(expresssion);
        
        public static implicit operator Expression<Func<T, string>>(Formatter<T> formatter) => formatter._expression;
        
        private readonly Expression<Func<T, string>> _expression;

        public Formatter(Expression<Func<T, string>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }
        
        public Formatter<TParent> From<TParent>(Expression<Func<TParent, T>> mapFrom)
            => _expression.From(mapFrom);
    }
}