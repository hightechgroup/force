using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain;
using Force.Cqrs;
using Force.Ddd;

namespace Demo.WebApp.Features.Accounts
{
    public class UpdateUserEmail: IValidatableCommand
    {    
        [Required]
        public Id<User> UserId { get; set; }
        
        [Required]
        public Email Email { get; set; }
    }
}