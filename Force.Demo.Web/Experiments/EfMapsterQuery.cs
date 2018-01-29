using System;
using System.Linq;
using System.Reflection;
using FastMember;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Demo.Domain;
using Force.Demo.Web.Shop.Catalog;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Force.Demo.Web
{
    public class EfMapsterQuery<TParam, TDto>
        : IQuery<TParam, PagedResponse<TDto>>
        
        where TParam: IPaging
        where TDto: class, IHasId
    {
        private readonly DbContext _dbContext;

        private static Type EntityType => typeof(TDto)
            .GetCustomAttribute<ProjectionAttribute>()
            .EntityType;
        
        public EfMapsterQuery()
        {
            //_dbContext = dbContext;
            
        }

        private static IQueryable<Product> _products = new Product[]
        {
            new Product("Штука", 100, new Category("Штуки", "things"){Id = 1}){Id = 1},
            new Product("Дрюка", 200, new Category("Дрюки", "drucs"){Id = 2}){Id = 2}
        }.AsQueryable();

        public PagedResponse<TDto> Ask(TParam spec)
            => _products
                .ProjectToType<TDto>()
                .OrderBy(x => x.Id)
                .ToPagedResponse(spec);
        
//            => ((IQueryable) typeof(DbContext)
//                    .GetMethods()
//                    .First(x => x.Name == "Set")
//                    .MakeGenericMethod(EntityType)
//                    .Invoke(_dbContext, new object[] { }))
//                .ProjectToType<TDto>()
//                .OrderBy(x => x.Id)
//                .ToPagedResponse(spec);
    }

    public class ProductDtoQuery : IQuery<int, ProductDto>
    {
        public ProductDto Ask(int spec)
        {
            return new ProductDto();
        }
    }
}