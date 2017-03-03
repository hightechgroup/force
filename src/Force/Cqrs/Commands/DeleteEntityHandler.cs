using System;
using Force.Ddd;
using Force.Ddd.Entities;

namespace Force.Cqrs.Commands
{
    public class DeleteEntityHandler<TKey, TEntity> : IHandler<TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEntityHandler(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            _unitOfWork = unitOfWork;
        }

        public void Handle(TKey key)
        {
            var entity = _unitOfWork.Find<TEntity>(key);
            if (entity == null)
            {
                throw new ArgumentException($"Entity {typeof(TEntity).Name} with id={key} doesn't exists");
            }

            _unitOfWork.Delete(entity);
            _unitOfWork.Commit();
        }

    }
}
