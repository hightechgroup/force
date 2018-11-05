using Demo.WebApp.Controllers;
using Force;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Accounts
{
    public class AccountController: ApiControllerBase
    {
        [HttpPut]
        public IActionResult Update(
            [FromServices] IHandler<UpdateAccountEmail> handler,
            [FromBody] UpdateAccountEmail command)
            => handler.ToResult(command);
    }
}