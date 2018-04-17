using System;
using DemoApp.Domain;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Core
{
    public class UserController : Controller
    {
        public IActionResult Refinement()
        {
            var refinement = new AdultRefinement();

            return Ok(new
            {
                Predicate = refinement.ToString(),
                ErrorMessage = refinement.ErrorMessage
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            return Ok(ModelState.IsValid);
        }
    }
}