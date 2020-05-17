using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Force.Cqrs
{
    public interface ICommand
    {        
    }
    
    public interface ICommand<T>
    {        
    }

    public interface IValidatableCommand : ICommand<IEnumerable<ValidationResult>>
    {        
    }
}