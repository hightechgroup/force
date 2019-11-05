using System.Collections.Generic;
using System.Linq;
using EFCore.BulkExtensions;
using Force.Extensions;
using Force.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Extensions;

namespace WebApplication.Features.Category
{
    public class CategoryController: Controller
    {
        public object Index2(IQueryable<Product> products)
        {
            DbContext _dbContext = null;
            _dbContext
                .Set<Product>()
                .Where(!Product.Specs.IsForSale)
                .BatchDelete();
            
            _dbContext
                .Set<Product>()
                .Where(!Product.Specs.IsForSale)
                .BatchUpdate(x => new Product{ Price = x.Price + 100 });
            
            return products
                .Where(Product.Specs.IsForSale)
                .ToList();
        }
        
        public IActionResult Index(
            [FromServices] IQueryable<Product> products,
            [FromQuery] CategoryFilterQuery query)
            => (products
                    .Where(Product.Specs.IsForSale)       
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Price
                    })
                    .ProjectToType<ProductListItem>()
                    .Filter(query)
                    .ToList() as IEnumerable<ProductListItem>)
                    .ToQueryResultViewModel(query)
                    .PipeTo(View);

        public IActionResult Display([FromServices] IQueryable<Product> querayble, int id)
            => querayble
                .ProjectToType<ProductDetails>()
                .FirstOrDefaultById(id)
                .ToViewOrNotFound(this);
    }
}