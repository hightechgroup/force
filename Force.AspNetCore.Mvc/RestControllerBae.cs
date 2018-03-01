using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Force.AspNetCore.Mvc
{
//    public abstract class RestControllerBase<TEntity, TListItem>
//        : RestControllerBase<long, SimplePaging, TEntity, TListItem, TListItem>
//        where TEntity : class, IHasId<long>
//        where TListItem : class, IHasId<long>
//    {
//    }

    public abstract class RestControllerBase<TEntity, TListParams, TInfo, TDetails>
        : RestControllerBase<long, TEntity, TListParams, TInfo, TDetails>
        where TListParams : IQueryableFilter<TInfo>, IQueryableOrder<TInfo>, IPaging
        where TEntity : class, IHasId<long>
        where TInfo : class, IHasId<long>
        where TDetails : class, IHasId<long>
    {
        protected RestControllerBase(IQueryable<TEntity> queryable, IUnitOfWork unitOfWork)
        : base(queryable, unitOfWork)
        {
        }
    }

    public abstract class RestControllerBase<TKey, TEntity, TListParams, TInfo, TDetails>
        : GetControllerBase<TKey, TEntity, TListParams, TInfo, TDetails>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TListParams : IQueryableFilter<TInfo>, IQueryableOrder<TInfo>, IPaging
        where TEntity : class, IHasId<TKey>
        where TInfo : class, IHasId<TKey>
        where TDetails : class, IHasId<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;

        protected RestControllerBase(IQueryable<TEntity> queryable, IUnitOfWork unitOfWork) : base(queryable)
        {
            _unitOfWork = unitOfWork;
        }

        protected abstract TKey SaveOrUpdate(TDetails model);

        [HttpPost]
        public virtual IActionResult Post([FromBody]TDetails model)
        {
            var id = SaveOrUpdate(model);
            return CreatedAtAction("Get", new { id });
        }

       
        [HttpPut("{id}")]
        public virtual IActionResult Put(TKey id, [FromBody] TDetails model)
        {
            SaveOrUpdate(model);
            return Ok(new { success = true });
        }


        [HttpDelete("{id}")]
        public virtual IActionResult Delete(TKey id)
        {
            var entity = _unitOfWork.Find<TEntity>(id);

            _unitOfWork.Remove(entity);
            _unitOfWork.Commit();

            return Ok(new { success = true });
        }
    }
}