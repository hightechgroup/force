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
                .Set<ActiveCart>()
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

            cart.Add(new Product()
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

            var cart2 = cart.Order();
            cart2.Total = 100505;
            dbContext.Entry(cart).State = EntityState.Detached;
            dbContext.Attach(cart2);
            dbContext.Entry(cart2).State = EntityState.Modified;
            dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }
            
    }

    public class AddToCart
    {
        public Id<Product> ProductId { get; set; }
    }
}