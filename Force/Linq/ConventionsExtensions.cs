using System.Linq;
using Force.Infrastructure;

namespace Force.Linq
{
    public static class ConventionsExtensions
    {
        public static IQueryable<TSubject> FilterAndSort<TSubject, TPredicate>(
            this IQueryable<TSubject> query, TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = Conventions<TSubject>.Filter(query, predicate, composeKind);
            var orderBy = Type<TPredicate>.PublicProperties.FirstOrDefault(x => x.Name == "OrderBy");
            var proprtyName = orderBy?.GetValue(predicate, null) as string;
            
            return proprtyName == null
                ? filtered
                : Conventions<TSubject>.Sort(filtered, proprtyName);
        }

        public static IOrderedQueryable<TSubject> OrderBy<TSubject>(this IQueryable<TSubject> query, string propertyName)
            => Conventions<TSubject>.Sort(query, propertyName);
    }
}