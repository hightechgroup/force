using System.Linq;
using AutoMapper.QueryableExtensions;
using Demo.WebApp.Controllers;
using Demo.WebApp.Domain;
using Force.Ddd;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Products
{
    public class ProductController: ApiControllerBase
    {
        public IActionResult Get([FromServices] IQueryable<Product> products, ProductListQuery productListQuery)
            => products
                .ProjectTo<ProductListDto>()
                .FilterAndSort(productListQuery)
                .ToList()
                .PipeTo(Ok);
    }
}