using System;
using Force.Cqrs;
using Force.Ddd;
using Force.Demo.Web.Experiments;
using Force.Demo.Web.Shop.Catalog;
using Microsoft.AspNetCore.Http;

namespace Force.Demo.Web
{
    public interface IResultDecorator<TSource, TDestination>
    {
        Result<TDestination> Decorate(Func<TSource, Result<TDestination>> func, TSource value);
    }
    
    public class ValidationResultDecorator<TSource, TDestination>: IResultDecorator<TSource, TDestination>
    {
        public Result<TDestination> Decorate(Func<TSource, Result<TDestination>> func, TSource value)
            => value.Validate()
                .Return(func, Result.Fail<TDestination>);
    }
    
    public class ValidationHandlerDecorator<TSource, TDestination>
        : ResultCommandQueryCommandHandlerDecorator<TSource, TDestination>
    {
        private readonly ValidationResultDecorator<TSource, TDestination> _decorator;

//        public ValidationCommandQueryDecorator(ValidationResultDecorator<TSource, TDestination> decorator,
//            IQuery<TSource, Result<TDestination>> query) : base(query)
//        {
//            _decorator = decorator;
//        }

        public ValidationHandlerDecorator(ValidationResultDecorator<TSource, TDestination> decorator, ICommandHandler<TSource, Result<TDestination>> commandHandler) : base(commandHandler)
        {
            _decorator = decorator;
        }

//        protected Result<TDestination> Decorate(TSource value)
//        {
//            => _decorator.Decorate(QueryHandler != null
//                ? (Func<TSource, Result<TDestination>>) _decorator.Ask
//                : Handler.Handle, value);
//        }

        protected override Result<TDestination> Decorate(Func<TSource, Result<TDestination>> func, TSource value)
        {
            throw new NotImplementedException();
        }
    }
    
    public class SecurityQueryResultDecorator<TSource, TDestination>
        : IResultDecorator<TSource, TDestination>
        where TDestination: IHasOwner
    {
        private readonly IHttpContextAccessor _accessor;

        public SecurityQueryResultDecorator(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Result<TDestination> Decorate(Func<TSource, Result<TDestination>> query, TSource value)
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Result.Fail<TDestination>(new SecurityFailure(""));
            }

            var currentUserName = _accessor.HttpContext.User.Identity.Name;
            return query(value).Check(
                x => x.UserName == currentUserName,
                x => new SecurityFailure(x.UserName));
        }
    }

    public interface IHasOwner
    {
        string UserName { get; }
    }
}