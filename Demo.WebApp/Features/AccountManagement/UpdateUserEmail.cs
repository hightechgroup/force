using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain;
using Demo.WebApp.Domain.Entities.Account;
using Force.Cqrs;
using Force.Ddd;

namespace Demo.WebApp.Features.AccountManagement
{
    public class UpdateUserEmail: IValidatableCommand
    {    
        [Required]
        public Id<User> UserId { get; set; }
        
        [Required]
        public Email Email { get; set; }
    }
}