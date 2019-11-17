using System.ComponentModel.DataAnnotations;
using System.Linq;
using DotNext.DddWorkshop.Areas.Products.Domain;
using DotNext.DddWorkshop.Infrastructure;
using Force.Ddd;
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

        public IActionResult ChangePrice([FromQuery]ChangePrice changePrice)
        {
            return View(changePrice);
        }
        
        
        public IActionResult Index([FromServices] DbContext dbContext)
            => dbContext
                .Set<Product>()
                .Where(Product.Specs.IsForSale)
                .ToList()
                .PipeTo(View);
    }
}