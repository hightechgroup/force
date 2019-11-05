using Force.Ddd;
using WebApplication.Extensions;

namespace WebApplication.Features.Category
{
    public class ProductSpecs: Specs<Product>
    {
        public Spec<Product> IsForSale { get; } = Spec(x => x.Price > 0);
    }
}