using DotNext.DddWorkshop.Models;

namespace DotNext.DddWorkshop.Areas.Products.Entities
{
    public class HasNameBase: HasIdBase
    {
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}