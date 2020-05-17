using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Force.Extensions;
using Force.Linq;

namespace Force.Tests.Infrastructure.Context
{
    [Display(Name = "Product")]
    public class Product : HasNameBase
    {
        [Required]
        public Category Category { get; protected set; }

        [SearchBy]
        [Required, MinLength(1), MaxLength(Strings.DefaultLength)]
        public override string Name { get; protected set; }

        protected Product() : base()
        {
        }

        public Product(Category category, string name) : base(name)
        {
            Category = category;
            this.EnsureInvariant();
        }
    }
}