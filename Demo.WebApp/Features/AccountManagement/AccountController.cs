using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Demo.WebApp.Controllers;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Demo.WebApp.Infrastructure;
using Force.Cqrs;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Features.AccountManagement
{
    public class AccountController: ApiControllerBase
    {
        private readonly IValidatableCommandHandler<UpdateUserEmail> _updateAccountEmailHandler;

        private static readonly Func<DemoAppDbContext, IEnumerable<Account>> CompiledQuery =
            EF.CompileQuery((DemoAppDbContext c) => c
                .Set<Account>()
                .AsNoTracking());
        
        //IValidatableCommandHandler<UpdateAccountEmail> updateAccountEmailHandler
        public AccountController()
        {
            //_updateAccountEmailHandler = updateAccountEmailHandler;
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateUserEmail command)
            => _updateAccountEmailHandler.ToResult(command);

        [HttpGet("test")]
        public IActionResult Test([FromServices] DemoAppDbContext demoAppDbContext, [FromQuery][Required]Email email)
        {
            return Ok(new
            {
                email,
                state = ModelState.IsValid
            });
        }
        
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