using System.Linq;
using AutoMapper.QueryableExtensions;
using Demo.WebApp.Controllers;
using Demo.WebApp.Domain;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Products
{
    public class ProductController: ApiControllerBase
    {
        private readonly IPagedQueryHandler<ProductListQuery, ProductListDto> _getProductList;

        public ProductController(IPagedQueryHandler<ProductListQuery, ProductListDto> getProductList)
        {
            _getProductList = getProductList;
        }

        public IActionResult Get(ProductListQuery productListQuery)
            => _getProductList
                .ToResult(productListQuery);
    }
}