using System.Linq;
using DotNext.DddWorkshop.Areas.Products.Entities;
using DotNext.DddWorkshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNext.DddWorkshop.Areas.Products
{
    public class ProductController : Controller
    {
        public IActionResult Init([FromServices] DbContext dbContext)
        {
            dbContext.Set<Product>().AddRange(new []
            {
                new Product()
                {
                    Name = "iPhone",
                    Price = 500
                },
                new Product()
                {
                    Name = "MacBook",
                    Price = 1000
                }
            });

            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public IActionResult Index([FromServices] DbContext dbContext)
            => dbContext
                .Set<Product>()
                .ToList()
                .PipeTo(View);
    }
}