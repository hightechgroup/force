using System;
using System.Linq.Expressions;

namespace Force.Ddd
{
    public class AutoSpec<T>: Spec<T> 
        where T : class, IHasId
    {
        private static Expression<Func<T, bool>> BuildExpression()
        {
            throw new NotImplementedException();
        }
        
        public AutoSpec() : base(BuildExpression())
        {
        }
    }
}