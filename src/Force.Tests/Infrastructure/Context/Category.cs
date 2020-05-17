using Force.Ddd;
using Force.Extensions;

namespace Force.Tests.Infrastructure.Context
{
    public class Category: HasNameBase
    {
        public Category(string name) : base(name)
        {
            this.EnsureInvariant();
        }
    }
}