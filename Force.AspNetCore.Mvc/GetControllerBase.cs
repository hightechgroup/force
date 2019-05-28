using System;
using System.Linq;
using Force.Ddd;
using Force.Extensions;
using Force.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Force.AspNetCore.Mvc
{
   public abstract class GetControllerBase<TKey, TEntity, TInfo, TDetails> : Controller
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class
        where TDetails : class, IHasId<TKey>
   {
        private readonly IQueryable<TEntity> _queryable;

        protected GetControllerBase(IQueryable<TEntity> queryable)
        {
            _queryable = queryable;
        }

        public abstract IQueryable<TInfo> ProjectInfo(IQueryable<TEntity> queryable);

        public abstract IQueryable<TDetails> ProjectDetails(IQueryable<TEntity> queryable);

        [HttpGet]
        public virtual IActionResult Get([FromQuery] PagedFetchOptions<TInfo> pagedFetchOptions)
            => _queryable
                .PipeTo(ProjectInfo)
                .FilterSortAndPaginate(pagedFetchOptions)
                .PipeTo(Ok);

        [HttpGet("{id}")]
        public virtual IActionResult Get(TKey id)
            => _queryable                
                .PipeTo(ProjectDetails)
                .ById(id)
                .PipeTo(Ok);
    }
}