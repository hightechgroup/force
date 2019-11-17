using DotNext.DddWorkshop.Infrastructure;

namespace DotNext.DddWorkshop.Areas.Products.Domain
{
    public class ProductSpecs: Specs<Product>
    {
        public readonly Spec<Product> IsForSale = Spec(x => x.Price > 0);
    }
}