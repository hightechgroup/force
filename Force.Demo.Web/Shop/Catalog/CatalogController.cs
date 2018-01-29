using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Demo.Data;
using Force.Demo.Domain;
using Force.Demo.Web.LinqToDb;
using Force.Extensions;
using Force.MvcCore;
using LinqToDB.Data;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Force.Demo.Web.Shop.Catalog
{
    //https://liiw.blogspot.ru/2017/02/ef-core-vs-linq2db.html
    //[Validate]
    public class CatalogController: Controller
    {
        private readonly IQueryable<Category> _categories;
        private readonly DemoContext _dc;
        private readonly DemoConnection _dataConnection;
        private readonly IHandler<ProductFilterParam, Result<PagedResponse<ProductDto>>> _handler;
        private readonly IQuery<ProductFilterParam, Result<PagedResponse<ProductDto>>> _productQuery;
        private readonly IQuery<int, Result<ProductDto>> _productDtoQuery;

        public CatalogController(
//            IQueryable<Category> categories,
//            DemoContext dc,
//            DemoConnection dataConnection,
            IHandler<ProductFilterParam, Result<PagedResponse<ProductDto>>> handler,
            IQuery<ProductFilterParam, Result<PagedResponse<ProductDto>>> productQuery,
            IQuery<int, Result<ProductDto>> productDtoQuery)
        {
//            _categories = categories;
//            _dc = dc;
//            _dataConnection = dataConnection;
            _handler = handler;
            _productQuery = productQuery;
            _productDtoQuery = productDtoQuery;
        }

        [Route("data")]
        public IActionResult Data()
        {
            //_dc.Categories.Add(new Category("Смартфоны", "smartphones"));
            //_dc.Categories.Add(new Category("Шины", "tires"));

            var cat = new Category("Штуки", "shtuki");
            //var pr = new Product("Продуктик", 100500, cat);
            _dc.Categories.Add(cat);
            _dc.SaveChanges();
            return Ok();
        }
        
        [Route("test-ef")]
        public IActionResult TestEf(ProductFilterParam param)
        {
            /*
                Exceptions:
                + Просто
                + Поддержка в языке
                - Не известно, что может пойти не так
                - Сигнал / шум
                
                Result
                - Не стандартно
                - Нужен монадический синтаксис
                + Видно, что может пойти не так
                + Нет нестинга
                + Уже есть тип возвращаемого значения
             */
            
            // Поискать исключения в проектах
            // Expression -> Dapper? Visitor? 
            // Expression - валидация. TComb
            // Связаться с Маратом Галеевым
            // DTO - в Query нет модели
            // Посмотреть HandleError
            
            // permanent redirect 302
            // staff only 404?
            // model binding 404, 415, 422
            // validation 404, 415 422 Result<T, ValidationFailure>
            // security 401, 403 Result<T, SecurityFailure>
            // business logic validation 422 Result<T, Failure>
            // проверка на 15% (фгис), закрытый 
            // response data 200 / tmp redirect 301
            throw new NotImplementedException();
        }

        [Route("test")]
        public IActionResult Test(ProductFilterParam param)
            => _productQuery
                .Ask(param)
                .OnFailure(x => SendEmail(x.Message))
                .ToActionResult();

        [Route("test2")]
        public IActionResult Test2()
            => _productDtoQuery
                .Ask(1)
                .ToActionResult();
         
        [Route("test3")]
        public IActionResult Test3(ProductFilterParam param)
            => _handler
                .Handle(param)
                .ToActionResult();
        
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
        
        public static void SendEmail(string cmd)
        {
                       
        }  
        
        public static void SendEmail2(string cmd)
        {
            throw new NotImplementedException();            
        } 
        
        public static Result<string> ChangeUserName(ChangeUserNameCommand cmd)
        {
            throw new NotImplementedException();
        }

        public static Result Query(ChangeUserNameCommand command) =>
            from validation in command.Validate()
            from updateDb in ChangeUserName(validation).OnSuccess(SendEmail)
            select updateDb;

        public static Result Imperative(ChangeUserNameCommand command)
        {
            var res = command.Validate();
            if (res.IsFaulted) return res;
            
            return ChangeUserName(command)
                .OnSuccess(x => SendEmail(x));
        } 
    }

    [Projection(typeof(Product))]
    public class ProductDto: HasIdBase<int>, IHasOwner
    {
        public int CategoryId { get; set; }
        
        public decimal Price { get; set; }
        
        public string UserName => "BigBoss";
    }

    public class IdDto
    {
        public int Id { get; set; }
    }

    public class NameDto: HasNameBase
    {        
    }
   
    public class ProductFilterParam: Spec<ProductDto>, IPaging
    {
        [Range(5, 10)]
        public decimal MaxPrice { get; set; }
        
        //[EntityId(typeof(Category))]
        public int CategoryId { get; set; }

        public ProductFilterParam() : base(x => true)
        {
        }

        public override Expression<Func<ProductDto, bool>> Expression =>
            x => x.CategoryId == CategoryId && x.Price <= MaxPrice;

        public int Page { get; set; } = 1;
        
        public int Take { get; set; }
    }

    public class ChangeUserNameCommand
    {
        
    }

    public class SomeEntity
    {
        public EntityId<int> Id { get; set; }
    }
}