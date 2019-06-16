using System;
using System.Diagnostics;
using Demo.WebApp.Controllers;
using Force.Ddd;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.AccountManagement
{
    public class AccountController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            ValueObject.TryParse("vo1", out ValueObject1 vo1);
            Console.WriteLine("Try Parse End");

            return Ok(new {val = vo1.Value});
        }
    }

    public class ValueObject1 : ValueObject<string>
    {
        public ValueObject1(string value) : base(value)
        {
        }
    }
    
    public class ValueObject2 : ValueObject<string>
    {
        public ValueObject2(string value) : base(value)
        {
        }
    }
}