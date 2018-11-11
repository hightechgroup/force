using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain;

namespace Demo.WebApp.Features.Accounts
{
    public class AccountDto
    {
        //[Required]
        public Email Email { get; set; }
    }
}