using System;
using Force.Common;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Extensions;

namespace Force.Cqrs.Commands
{
    public class CreateOrUpdateEntityHandler<TKey, TCommand, TEntity>: IHandler<TCommand, TKey>
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

        public TKey Handle(TCommand command)
        {
            var id = (command as IHasId)?.Id;
            var entity = id != null && id.Equals(default(TKey)) == false
                ? _mapper.Map(command, _unitOfWork.Find<TEntity>(id))
                : _mapper.Map<TEntity>(command);

            if (entity.IsNew())
            {
                _unitOfWork.Add(entity);
            }

            _unitOfWork.Commit();
            return entity.Id;
        }
    }
}
