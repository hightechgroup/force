using System;
using System.Linq;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Tests.Stubs;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class InfrastructureExtensionsTests
    {
        [Fact]
        public void Is_True()
        {
            Assert.True("123".Is(x => int.Parse(x) > 1));
        }

        [Fact]
        public void Is_False()
        {
            Assert.False("123".Is(x => int.Parse(x) > 1000));
        }

        [Fact]
        public void DoubleInsert_Are_Equal()
        {
            var first = "123".Is(x => int.Parse(x) > 1000);
            var second = "123".Is(x => int.Parse(x) > 1000);
            Assert.Equal(first, second);
        }

        [Fact]
        public void Forward_Func_Query_CommandSuccess()
        {
            var res = "123"
                .PipeTo(x => x)
                .PipeTo(new SimpleQuery())
                .PipeTo(new SimpleCommandHandler());

            Assert.Equal("123", res);
            res.PipeTo(new SimpleCommandHandler2());
        }

        [Fact]
        public void Forward_Async()
        {
            var res = "123".PipeTo(new SimpleAsyncQuery()).Result;
            Assert.Equal("123", res);
        }
        
        [Fact]
        public void ToFunc_Query()
        {
            var q = new SimpleQuery();
            var func = q.ToFunc();
            Assert.Equal(q.Ask("123"), func("123"));
        }

        [Fact]
        public void ToFunc_CommandHandler()
        {
            var q = new SimpleCommandHandler();
            var func = q.ToFunc();
            Assert.Equal(q.Handle("123"), func("123"));
        }
        
        [Fact]
        public void WhereIf_True_Applied()
        {
            var strings = new[] {"1", "2", "3"}.AsQueryable();
            var res = strings.WhereIf(true, x => x != "1").ToArray();
            Assert.Equal(2, res.Length);
        }

        [Fact]
        public void WhereIf_False_MotApplied()
        {
            var strings = new[] { "1", "2", "3" }.AsQueryable();
            var res = strings.WhereIf(false, x => x != "1").ToArray();
            Assert.Equal(3, res.Length);
        }

        [Fact]
        public void Queryable_ById()
        {
            var strings = new[] { new Product() {Id = 1}, new Product() {Id = 2},  }.AsQueryable();
            var id1 = strings.ById(1).Id;
            var id2 = strings.ById(2).Id;

            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
        }

        [Fact]
        public void LinqProvider_ById()
        {
            var provider = new InMemoryLinqProvider(new[]
            {
                new Product() {Id = 1}, new Product() {Id = 2},
            });

            
            var id1 = provider.ById<Product>(1).Id;
            var id2 = provider.ById<Product>(2).Id;

            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
        }

        [Fact]
        public void AskSync()
        {
            var res = new SimpleAsyncQuery().AskSync("123");
            Assert.Equal("123", res);
        }

        [Fact]
        public void RunTask()
        {
            Func<string> func = () => "123";
            var res = func.RunTask().Result;
            Assert.Equal("123", res);
        }

        [Fact]
        public void Either_True()
        {
            var res = "123".Either(x => true, x => x + "456", x => "");
            Assert.Equal("123456", res);
        }

        [Fact]
        public void Either_False()
        {
            var res = "123".Either(x => false, x => x + "456", x => "");
            Assert.Equal("", res);
        }

        [Fact]
        public void Apply()
        {
            var products = new[] { new Product(), new Product() {Price = 100500},  }
                .AsQueryable()
                .Where(new UberProductSpec())
                .ToArray();

            Assert.Equal(1, products.Length);
            Assert.Equal(100500, products.First().Price);
        }

        [Fact]
        public void Compose()
        {
            Func<string, int> strtoi = int.Parse;
            Func<int, string> itostr = x => x.ToString();
            var func = strtoi.Compose(itostr);
            var res = func("123");

            Assert.Equal("123", res);
        }
    }
}
