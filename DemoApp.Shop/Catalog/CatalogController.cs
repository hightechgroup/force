using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using DemoApp.Admin.CatalogAdmin;
using DemoApp.Domain;
using Force;
using Force.AspNetCore.Mvc;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Demo.Web;
using Force.Extensions;
using Force.Meta;
using Force.Meta.Validation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Shop.Catalog
{
    public class CatalogController : Controller
    {
        private readonly DbContext _dbContext;

        public CatalogController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IActionResult Index([Bind(Prefix = "ProductListParams")]ProductListParams listParams)
        {
            var products = _dbContext
                .Set<Product>()
                .ProjectToType<ProductListItem>()
                .AutoFilter(listParams)
                .ToList();
            
            return View(new ProductListViewModel(listParams, products));
        }
    }

    public class ProductListViewModel
    {
        public ProductListViewModel(ProductListParams productListParams, IEnumerable<ProductListItem> products)
        {
            Products = products;
            ProductListParams = productListParams;
        }

        public IEnumerable<ProductListItem>  Products { get; protected set; }
        
        public ProductListParams ProductListParams { get; protected set; }               
    }
}