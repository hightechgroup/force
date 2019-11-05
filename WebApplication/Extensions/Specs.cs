using System;
using System.Linq.Expressions;
using Force.Ddd;

namespace WebApplication.Extensions
{
    public abstract class Specs<T>
    {
        protected static Spec<T> Spec(Expression<Func<T, bool>> expr) => new Spec<T>(expr);
    }
}