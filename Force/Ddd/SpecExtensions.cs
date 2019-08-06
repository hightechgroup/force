using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Expressions;
using Force.Extensions;
using Force.Infrastructure;
using Force.Linq;

namespace Force.Ddd
{
    public static class SpecExtensions
    {
        public static Spec<T> ToSpec<T>(this Expression<Func<T, bool>> expr)
            where T : class, IHasId
            => new Spec<T>(expr);
    }
}