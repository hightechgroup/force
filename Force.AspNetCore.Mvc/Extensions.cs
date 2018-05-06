using System.Linq;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Force.AspNetCore.Mvc
{
    public static class Extensions
    {
        public static PagedResponse<TProjection> SmartPaging<TEntity, TProjection>(this IQueryable<TEntity> query, 
            ISmartPaging<TEntity, TProjection> smartPaging)
            where TEntity : class, IHasId
            where TProjection : class, IHasId => query
            .PipeToIf(_ => smartPaging.Spec != null, x => x.Where(smartPaging.Spec))
            .ProjectToType<TProjection>()
            .PipeToIf(_ => smartPaging.Filter != null, x => x.Where(smartPaging.Filter))
            .PipeToIf(_ => smartPaging.Order != null, x => x.OrderBy(smartPaging.Order))
            .OrderByIdIfNotOrdered()
            .ToPagedResponse(smartPaging);
        
        public static IActionResult ToActionResult<T>(this Result<T> result)
            => result.Return<IActionResult>(
                x => new OkObjectResult(x),
                x => new FailedResult(x));
    }
}