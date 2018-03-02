using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Force.AspNetCore.Mvc;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Force.AspNetCore.Mvc
{
    public abstract class RestControllerBase<TKey, TEntity>
        : RestControllerBase<TKey, TEntity, SimpleParams<TEntity>, TEntity, TEntity>
    
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>, IHasId<int>
    {
        protected RestControllerBase(IQueryable<TEntity> queryable, IUnitOfWork unitOfWork)
            : base(queryable, unitOfWork)
        {
        }
    }

    public abstract class RestControllerBase<TKey, TEntity, TListParams, TInfo>
        : RestControllerBase<TKey, TEntity, TListParams, TInfo, TEntity>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TListParams : IQueryableFilter<TInfo>, IQueryableOrder<TInfo>, IPaging
        where TEntity : class, IHasId<TKey>
        where TInfo : class, IHasId<TKey>
    {
        protected RestControllerBase(IQueryable<TEntity> queryable, IUnitOfWork unitOfWork)
            : base(queryable, unitOfWork)
        {
        }

        public override IActionResult Post(
            [FromBody, ModelBinder(typeof(EntityModelBinder))]
            TEntity model)
        {
            return base.Post(model);
        }

        public override IActionResult Put(
            TKey id,
            [FromBody, ModelBinder(typeof(EntityModelBinder))]
            TEntity model)
        {
            return base.Put(id, model);
        }

        protected override TKey SaveOrUpdate(TEntity model)
        {
            // Entity is fetched via ORM. So no need to set properties twice for update
            if (model.Id.Equals(default(TKey)))
            {
                UnitOfWork.Add(model);
            }

            UnitOfWork.Commit();
            return model.Id;
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
        protected readonly IUnitOfWork UnitOfWork;

        protected RestControllerBase(IQueryable<TEntity> queryable, IUnitOfWork unitOfWork)
            : base(queryable)
        {
            UnitOfWork = unitOfWork;
        }

        protected abstract TKey SaveOrUpdate(TDetails model);

        [HttpPost]
        public virtual IActionResult Post([FromBody] TDetails model)
        {
            var id = SaveOrUpdate(model);
            return CreatedAtAction("Get", new {id});
        }


        [HttpPut("{id}")]
        public virtual IActionResult Put(TKey id, [FromBody] TDetails model)
        {
            SaveOrUpdate(model);
            return Ok(new {success = true});
        }


        [HttpDelete("{id}")]
        public virtual IActionResult Delete(TKey id)
        {
            var entity = UnitOfWork.Find<TEntity>(id);

            UnitOfWork.Remove(entity);
            UnitOfWork.Commit();

            return Ok(new {success = true});
        }
    }
}