using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Force.Cqrs
{
    public interface ICommandHandler<in T>: IHandler<T>
        where T: ICommand
    {        
    }
    
    public interface ICommandHandler<in TIn, out TOut>: IHandler<TIn, TOut>
        where TIn: ICommand<TOut>
    {        
    }

    public interface IValidatableCommandHandler<in T> : ICommandHandler<T, IEnumerable<ValidationResult>>
        where T: IValidatableCommand
    {        
    }
}