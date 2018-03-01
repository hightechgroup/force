using System;
using System.Linq;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Force.Meta;
using Mapster;

namespace Force.AspNetCore.Mvc
{
    public abstract class GetControllerBase<TEntity, TListParams, TInfo, TDetails> 
        : GetControllerBase<long, TEntity, TListParams, TInfo, TDetails>
        where TListParams : IQueryableFilter<TInfo>, IQueryableOrder<TInfo>, IPaging
        where TEntity : class, IHasId<long>
        where TInfo : class, IHasId<long>
        where TDetails : class, IHasId<long>
    {
        protected GetControllerBase(IQueryable<TEntity> queryable)
            : base(queryable)
        {
        }
    }

    public abstract class GetControllerBase<TKey, TEntity, TListParams, TInfo, TDetails> : Controller
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TListParams : IQueryableFilter<TInfo>, IQueryableOrder<TInfo>, IPaging
        where TEntity : class, IHasId<TKey>
        where TInfo : class, IHasId<TKey>
        where TDetails : class, IHasId<TKey>
    {
        private readonly IQueryable<TEntity> _queryable;

        protected GetControllerBase(IQueryable<TEntity> queryable)
        {
            _queryable = queryable;
        }

        public virtual IActionResult ListMeta() => Ok(MetaProvider<TEntity>.List);

        [HttpGet]
        public virtual IActionResult Get([FromQuery] TListParams listParams)
            => _queryable
                .ProjectToType<TInfo>()    
                .Where(listParams)
                .OrderBy(listParams)
                .ToPagedResponse(listParams)
                .PipeTo(Ok);

        [HttpGet("{id}")]
        public virtual IActionResult Get(TKey id)
            => _queryable                
                .ProjectToType<TInfo>()
                .ById(id)
                .PipeTo(Ok);
    }
}