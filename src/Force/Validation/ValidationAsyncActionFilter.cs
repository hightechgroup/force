// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.Linq;
// using System.Threading.Tasks;
// using Force.Ccc;
// using Force.Cqrs;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
//
// namespace Infrastructure
// {
//     public class ValidationAsyncActionFilter: IAsyncActionFilter
//     {
//         private static Type[] _types = {
//             typeof(ICommand<>),
//             typeof(IQuery<>)
//         };
//         
//         public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//         {
//             CheckModelState(context);
//             RunValidators(context);
//
//             return CheckModelState(context) 
//                 ? next()
//                 : Task.CompletedTask;
//         }
//
//         private static void RunValidators(ActionExecutingContext context)
//         {
//             foreach (var arg in context.ActionArguments)
//             {
//                 var argType = arg.Value.GetType();
//                 foreach (var inf in argType.GetInterfaces())
//                 {
//                     if (typeof(ICommand) == inf || 
//                         inf.IsGenericType && _types.Contains(inf.GetGenericTypeDefinition()))
//                     {
//                         var validatorType = typeof(IValidator<>).MakeGenericType(argType);
//                         // TODO: add async validators
//                         dynamic v = context.HttpContext.RequestServices.GetService(validatorType);
//                         if (v != null)
//                         {
//                             IEnumerable<ValidationResult> res = v.Validate((dynamic)arg.Value);
//                             var errors = res.ToList();
//                             if (!errors.IsValid())
//                             {
//                                 context.ModelState.AddModelError(arg.Key,
//                                     errors
//                                         .Select(x => $"{x.MemberNames}: {x.ErrorMessage}")
//                                         .Aggregate((c, n) => $"{c}{Environment.NewLine}{n}"));
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//
//         private static bool CheckModelState(ActionExecutingContext context)
//         {
//             if (!context.ModelState.IsValid)
//             {
//                 context.Result = new ObjectResult(new ValidationResultModel(context.ModelState))
//                 {
//                     StatusCode = 422
//                 };
//
//                 return false;
//             }
//
//             return true;
//         }
//     }
// }