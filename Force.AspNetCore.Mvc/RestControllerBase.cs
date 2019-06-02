using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Force.AspNetCore.Mvc;
using Force.Ddd;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Force.AspNetCore.Mvc
{
    public abstract class RestControllerBase<TKey, TEntity, TInfo, TDetails>
        : GetControllerBase<TKey, TEntity, TInfo, TDetails>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId
        where TInfo : class
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
            // ReSharper disable once Mvc.ActionNotResolved
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