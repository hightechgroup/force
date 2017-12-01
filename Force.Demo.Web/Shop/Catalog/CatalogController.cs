using System.Linq;
using Force.Demo.Domain;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Force.Demo.Web.Shop.Catalog
{
    public class CatalogController: Controller
    {
        private readonly IQueryable<Category> _categories;

        public CatalogController(IQueryable<Category> categories)
        {
            _categories = categories;
        }

        [Route("test")]
        public IActionResult Test(TestParam param)
        {
            return Ok($"{param.A} {param.B}");
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

    [ModelBinder(typeof(ImmutableModelBinder))]
    public class TestParam
    {
        public TestParam(int a, int b)
        {
            A = a;
            B = b;
        }

        public int A { get; protected set; }
        
        public int B { get; protected set; }
    }
}