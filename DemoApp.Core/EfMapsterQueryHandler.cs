using System;
using System.Linq;
using System.Reflection;
using DemoApp.Domain;
using Force;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Core
{
    public class EfMapsterQueryHandler<TParam, TDto>
        : IQueryHandler<TParam, PagedResponse<TDto>>
        , IQueryHandler<int, TDto>
        
        where TParam: IPaging
        where TDto: class, IHasId
    {
        private readonly DbContext _dbContext;

        private static Type EntityType => typeof(TDto)
            .GetCustomAttribute<ProjectionAttribute>()
            .EntityType;
        
        public EfMapsterQueryHandler()
        {
            //_dbContext = dbContext;           
        }

        private static IQueryable<Product> _products = new Product[]
        {
            new Product("Штука", 100, new Category("Штуки", "things"){Id = 1}){Id = 1},
            new Product("Дрюка", 200, new Category("Дрюки", "drucs"){Id = 2}){Id = 2}
        }.AsQueryable();

        public PagedResponse<TDto> Handle(TParam param)
            => _products
                .ProjectToType<TDto>()
                .OrderBy(x => x.Id)
                .ToPagedResponse(param);


        public TDto Handle(int input)
        {
            throw new NotImplementedException();
        }
    }
}