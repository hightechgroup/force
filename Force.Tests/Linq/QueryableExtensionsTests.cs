using System.Linq;
using Force.Linq;
using Xunit;

namespace Force.Tests.Linq
{
    public class QueryableExtensionsTests
    {
        private static readonly string[] Strings =  {"1", "2", "3"};

        public IQueryable<string> Queryable = Strings.AsQueryable();
        
        [Fact]
        public void LeftJoin()
        {
            Queryable.LeftJoin(Queryable, 
                x => x, 
                x => x, 
                (x,y) => x);
        }

        [Fact]
        public void ById()
        {
            
        }

        [Fact]
        public void WhereIf()
        {
            Queryable.WhereIf(true, x => true);
            Queryable.WhereIf(false, x => true);
            
            Queryable.WhereIf(true, x => true, x => false);
        }

        [Fact]
        public void OrderBy()
        {
            // try?
            Queryable.OrderBy("Length");
        }
        
        [Fact]
        public void OrderByDescending()
        {
            Queryable.OrderByDescending("Length");
        }
    }
}