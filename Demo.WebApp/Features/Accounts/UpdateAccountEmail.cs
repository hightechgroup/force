using System.ComponentModel.DataAnnotations;
using Force.Cqrs;

namespace Demo.WebApp.Features.Accounts
{
    public class UpdateAccountEmail: IValidatableCommand
    {        
        [Required]
        public string Email { get; set; }
    }
}