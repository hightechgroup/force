using Demo.WebApp.Controllers;
using Force;
using Force.Cqrs;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Accounts
{
    public class AccountController: ApiControllerBase
    {
        private readonly IValidatableCommandHandler<UpdateAccountEmail> _updateAccountEmailHandler;

        public AccountController(IValidatableCommandHandler<UpdateAccountEmail> updateAccountEmailHandler)
        {
            _updateAccountEmailHandler = updateAccountEmailHandler;
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateAccountEmail command)
            => _updateAccountEmailHandler.ToResult(command);
    }
}