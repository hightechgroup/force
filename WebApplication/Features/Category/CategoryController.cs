using System.Collections.Generic;
using System.Linq;
using Force.Extensions;
using Force.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Features.Category
{
    public class CategoryController: Controller
    {
        public IActionResult Index(
            [FromServices] IQueryable<Product> products,
            [FromQuery] CategoryFilterQuery query)
            => (products
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