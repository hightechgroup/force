using System;
using Force.Common;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Extensions;

namespace Force.Cqrs.Commands
{
    /// <summary>
    /// Command handler for generic Create/Update entity operations
    /// </summary>
    /// <typeparam name="TKey">Id Type</typeparam>
    /// <typeparam name="TCommand">Create or update entity command type</typeparam>
    /// <typeparam name="TEntity">Target entity type</typeparam>
    public class CreateOrUpdateEntityHandler<TKey, TCommand, TEntity>:
        IHandler<TCommand, TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrUpdateEntityHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Create new entity or update existing. If command is IHasId&lt;T&gt; handler tries to load
        /// existing using IUnitOfWork. Otherwise create new Entity
        /// </summary>
        /// <param name="command">Create or update entity command</param>
        /// <exception cref="InvalidOperationException">Occurs when trying delete not existing entity</exception>
        /// <returns>Modified entity Id</returns>
        public TKey Handle(TCommand command)
        {
            var id = (command as IHasId)?.Id;
            TEntity entity = null;
            if (id != null && id.Equals(default(TKey)) == false)
            {
                var existing = _unitOfWork.Find<TEntity>(id);
                if (existing == null)
                {
                    throw new InvalidOperationException($"Entity {typeof(TEntity).Name} with id={id} doesn't exists");
                }

                entity = _mapper.Map(command, _unitOfWork.Find<TEntity>(id));
            }
            else
            {
                entity = _mapper.Map<TEntity>(command);
            }

            if (entity.IsNew())
            {
                _unitOfWork.Add(entity);
            }

            _unitOfWork.Commit();
            return entity.Id;
        }
    }
}
