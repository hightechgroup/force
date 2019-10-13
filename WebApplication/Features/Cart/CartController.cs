using System;
using System.Linq;
using Force.Ddd;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Features.Cart.Entities;

namespace WebApplication.Features.Cart
{
    public class CartController: Controller
    {
        public IActionResult Index([FromServices] DbContext dbContext)
            => dbContext
                .Set<Entities.ActiveCart>()
                .Include(x => x.CartItems)
                .ToList()
                .PipeTo(View);


        public IActionResult Add(
            [FromServices] DbContext dbContext,
            AddToCart command)
        {
            var cart = dbContext
                .Set<ActiveCart>()
                .FirstOrDefault()
                ?? new ActiveCart();

            if (cart.IsNew())
            {
                dbContext.Add(cart);
            }

            cart.Add(new Category.Product()
            {
                Name = "Product",
                Price = 100500
            });

            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Order(
            [FromServices] DbContext dbContext)
        {
            var cart = dbContext
                .Set<ActiveCart>()
                .FirstOrDefault();

            cart.Order();
            
            return RedirectToAction("Index");
        }
            
    }

    public class AddToCart
    {
        public Id<Category.Product> ProductId { get; set; }
    }
}