using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoMapper.Extensions;
using Force.AutoMapper;
using Force.Cqrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Force.AspNetCore.Mvc
{
    public static class HandlerExtensions
    {
        public static IActionResult HandleCommand<T>(this ControllerBase controller, T input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var handler = (IHandler<T, IEnumerable<ValidationResult>>) controller
                .HttpContext
                .RequestServices
                .GetService(typeof(IHandler<T, IEnumerable<ValidationResult>>));

            if (handler == null)
            {
                var handler2 = (IHandler<T>) controller
                    .HttpContext
                    .RequestServices
                    .GetService(typeof(IHandler<T>));

                if (handler2 == null)
                {
                    throw new InvalidOperationException(
                        $"Handler for type \"{typeof(T)}\" is not found");
                }

                handler2.Handle(input);
                return new OkResult();
            }

            var res = handler.Handle(input);
            return res == null
                ? (IActionResult) new OkResult()
                : new OkObjectResult(res);
        }

        public static ActionResult<T> Fetch<T>(this ControllerBase controller, IQuery<T> input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var inputType = input.GetType();

            dynamic handler = controller
                .HttpContext
                .RequestServices
                .GetService(typeof(IHandler<,>).MakeGenericType(inputType, typeof(T)));

            if (handler == null)
            {
                throw new InvalidOperationException(
                    $"Handler for type \"{input.GetType()}\" is not found");
            }

            dynamic dynamicInput = input;
            var res = handler.Handle(dynamicInput);
            return new ActionResult<T>(res);
        }

        public static ActionResult<IEnumerable<T>> FetchEnumerable<T>(this ControllerBase controller,
            IQuery<IEnumerable<T>> input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var inputType = input.GetType();

            dynamic handler = controller
                .HttpContext
                .RequestServices
                .GetService(typeof(IHandler<,>).MakeGenericType(inputType, typeof(IEnumerable<T>)));

            if (handler == null)
            {
                var projectionAttr = typeof(T).GetCustomAttribute<ProjectionAttribute>();
                if (projectionAttr == null)
                {
                    var type = input.GetType();
                    throw new InvalidOperationException(
                        $"Handler for type \"{type}>\" is not found. {type} has no Projection attribute as well. " +
                        $"Either register a handler or mark {type} with Projection attribute");
                }

                var handlerType =
                    typeof(LinqQueryHandler<,,>).MakeGenericType(projectionAttr.Type, inputType, typeof(T));

                var dbContext = (DbContext) controller
                    .HttpContext
                    .RequestServices
                    .GetService(typeof(DbContext));

                var set = typeof(DbContext)
                    .GetMethod("Set")
                    .MakeGenericMethod(projectionAttr.Type)
                    .Invoke(dbContext, new object[] { });

                handler = Activator.CreateInstance(handlerType, set);
            }

            dynamic dynamicInput = input;
            var res = handler.Handle(dynamicInput);
            return new ActionResult<IEnumerable<T>>(res);
        }
    }
}