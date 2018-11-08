using System.Linq;
using Demo.WebApp.Controllers;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Force.Cqrs;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Accounts
{
    public class AccountController: ApiControllerBase
    {
        private readonly IValidatableCommandHandler<UpdateAccountEmail> _updateAccountEmailHandler;

        //IValidatableCommandHandler<UpdateAccountEmail> updateAccountEmailHandler
        public AccountController()
        {
            //_updateAccountEmailHandler = updateAccountEmailHandler;
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateAccountEmail command)
            => _updateAccountEmailHandler.ToResult(command);

        [HttpGet]
        public IActionResult Get([FromServices] DemoAppDbContext demoAppDbContext)
        {
            var email = new Email("max@hightech.group");
            
            return demoAppDbContext
                .Set<Account>()        
                .Where(x => x.Email == email)
                .ToList()
                .PipeTo(Ok);
        }
    }
}