using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force.Ddd;
using Force.Demo.Data;
using Force.Demo.Domain;
using Force.Demo.Web.LinqToDb;
using Force.Extensions;
using LinqToDB.Data;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Force.Demo.Web.Shop.Catalog
{
    //https://liiw.blogspot.ru/2017/02/ef-core-vs-linq2db.html
    [Validate(ValidationResult.View)]
    public class CatalogController: Controller
    {
        private readonly IQueryable<Category> _categories;
        private readonly DemoContext _dc;
        private readonly DemoConnection _dataConnection;

        public CatalogController(IQueryable<Category> categories, DemoContext dc, DemoConnection dataConnection)
        {
            _categories = categories;
            _dc = dc;
            _dataConnection = dataConnection;
        }

        [Route("data")]
        public IActionResult Data()
        {
            //_dc.Categories.Add(new Category("Смартфоны", "smartphones"));
            //_dc.Categories.Add(new Category("Шины", "tires"));

            var cat = new Category("Штуки", "shtuki");
            var pr = new Product("Продуктик", 100500, cat);
            _dc.Products.Add(pr);
            _dc.SaveChanges();
            return Ok();
        }
        
        [Route("test-ef")]
        public IActionResult TestEf(TestParam param)
        {
            var products = _dc.Products.ProjectToType<NameDto>().ToList();
            return View("test", products);
        }
        
        [Route("test-linq-to-db")]
        public IActionResult TestLinqToDb(TestParam param)
        {
            var products = _dataConnection.Products.ProjectToType<NameDto>().ToList();
            return View("test", products.ToList());
        }

        [Route("test-dapper")]
        public IActionResult TestLinqToDb()
        {
            return Ok();
        }

        [Route("catalog")]
        public IActionResult Index()
            => _categories
                .ToList()
                .PipeTo(View);

        [Route("catalog/{url}")]
        public IActionResult Index(string url)
            => _categories
                .FirstOrDefault(x => x.Url == url)
                .Either(Ok, _ => (IActionResult) NotFound());
    }

    public class IdDto
    {
        public int Id { get; set; }
    }

    public class NameDto: HasNameBase
    {        
    }
   
    public class TestParam
    {
        [Required]
        public int A { get; set; }
        
        [Required]
        public int B { get; set; }
    }

    public class SomeEntity
    {
        public EntityId<int> Id { get; set; }
    }
}