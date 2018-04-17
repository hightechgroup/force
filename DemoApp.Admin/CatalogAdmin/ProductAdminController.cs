using System.Linq;
using DemoApp.Domain;
using Force.AspNetCore.Mvc;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace DemoApp.Admin.CatalogAdmin
{
    public class ProductAdminController
        //: RestControllerBase<int, Product, ProductListParams, ProductListItem>
    {
//        public ProductAdminController(IQueryable<Product> queryable, IUnitOfWork unitOfWork) 
//            : base(queryable, unitOfWork)
//        {
//        }
    }

    public class ProductListParams : Paging<ProductListItem>, IQueryableFilter<ProductListItem>
    {
        public override IOrderedQueryable<ProductListItem> Order(IQueryable<ProductListItem> queryable)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ProductListItem> Filter(IQueryable<ProductListItem> query)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProductListItem : ProductBase
    {
    }
}