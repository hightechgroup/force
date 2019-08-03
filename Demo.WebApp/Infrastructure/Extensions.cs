using System;
using System.Threading.Tasks;
using Force;
using Force.Cqrs;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Infrastructure
{
    public static class Extensions
    {
        public static IActionResult ToResult<T>(this IHandler<T> handler, T input)
        { 
            handler.Handle(input);
            return new OkResult();
        }
        
        public static IActionResult ToResult<TIn, TOut>(this IHandler<TIn, TOut> handler, TIn input)
            => new OkObjectResult(handler.Handle(input));
    }
}