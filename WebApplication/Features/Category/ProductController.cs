using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Features.Cart;

namespace WebApplication.Features
{
    public class ProductController: Controller
    {
        public IActionResult AddToCart(
            [FromServices] DbContext dbContext,
            AddToCart command)
        {
            return Json(new {x = 1});
        }
    }
}