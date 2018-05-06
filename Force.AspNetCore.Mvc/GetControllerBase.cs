using System;
using System.Linq;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace Force.AspNetCore.Mvc
{
    public abstract class GetControllerBase<TEntity, TSmartPaging, TInfo, TDetails> 
        : GetControllerBase<long, TEntity, TSmartPaging, TInfo, TDetails>
        where TSmartPaging : ISmartPaging<TEntity, TInfo>
        where TEntity : class, IHasId<long>
        where TInfo : class, IHasId<long>
        where TDetails : class, IHasId<long>
    {
        protected GetControllerBase(IQueryable<TEntity> queryable)
            : base(queryable)
        {
        }
    }

    public abstract class GetControllerBase<TKey, TEntity, TSmartPaging, TInfo, TDetails> : Controller
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TSmartPaging : ISmartPaging<TEntity, TInfo>
        where TEntity : class, IHasId<TKey>
        where TInfo : class, IHasId<TKey>
        where TDetails : class, IHasId<TKey>
    {
        private readonly IQueryable<TEntity> _queryable;

        protected GetControllerBase(IQueryable<TEntity> queryable)
        {
            _queryable = queryable;
        }
       
        [HttpGet]
        public virtual IActionResult Get([FromQuery] TSmartPaging smartPaging)
            => _queryable
                .SmartPaging(smartPaging)
                .PipeTo(Ok);

        [HttpGet("{id}")]
        public virtual IActionResult Get(TKey id)
            => _queryable                
                .ProjectToType<TDetails>()
                .ById(id)
                .PipeTo(Ok);
    }
}