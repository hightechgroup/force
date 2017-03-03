using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Components;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Pagination;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Tests.Stubs;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Linq.Dynamic.Core;


namespace CostEffectiveCode.Tests
{
    public class Tests
    {
        //public Tests(ITestOutputHelper atr)
        //{
        //    //DotMemoryUnitTestOutput.SetOutputMethod(atr.WriteLine);
        //}

        static Tests()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Product, ProductDto>());
        }

        //[Fact]
        public void PagedQuery_Execute()
        {
            var pagedQuery = new PagedQuery<Product, ProductDto>(
                LinqProvider(), new StaticAutoMapperWrapper());

            var sw = new Stopwatch();

            sw.Start();
            var res = pagedQuery.Ask(new UberProductSpec() {Price = 3500});
            sw.Stop();
            
            Assert.Equal(100500, res.First().Price);
            Assert.Equal(1, res.TotalCount);
            Assert.True(sw.ElapsedMilliseconds < 150, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        private static InMemoryLinqProvider LinqProvider()
        {
            var category = new Category(100, "Smarphones");
            var linqProvider = new InMemoryLinqProvider(
                new[] {new Product(category, "iPhone", 100500), new Product(category, "Galaxy s7", 3500)});
            return linqProvider;
        }

        //[Fact]
        public void Async_Result()
        {
            var res = new OptimizedQuery().Ask(new UberProductSpec()).Result;
            Assert.Equal(2, res.TotalCount);
        }

        //[Fact]
        public async void Async_Await()
        {
            var res = await new OptimizedQuery().Ask(new UberProductSpec());
            Assert.Equal(2, res.TotalCount);
        }


        //[Fact]
        public void OptimizedQuery_Ask()
        {
            var pagedQuery = new OptimizedQuery();
            var sw = new Stopwatch();

            sw.Start();
            var res = pagedQuery.AskSync(new UberProductSpec() { Price = 3500 });
            sw.Stop();

            Assert.Equal(100500, res.First().Price);
            Assert.Equal(1, res.TotalCount);
            Assert.True(sw.ElapsedMilliseconds < 120, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        //[Fact]
        public void TestDbContext_PagedQuery_Ask()
        {
            var pagedQuery = new PagedQuery<Product, ProductDto>(
                new TestDbContext(), new StaticAutoMapperWrapper());

            var optimizedQUery = new OptimizedQuery();

            var sw = new Stopwatch();

            sw.Start();

            var specs = new List<UberProductSpec>();
            for (var i = 0; i < 50000; i++)
            {
                specs.Add(new UberProductSpec(1, 1) {Price = i});
            }

            var res = specs.Select(x => pagedQuery.Ask(x)).ToArray();

            sw.Stop();

            //Assert.Equal(99900, res.First().Price);
            //Assert.Equal(964, res.TotalCount);
            Assert.True(sw.ElapsedMilliseconds < 300, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");

        }

        //[Fact]
        public void TestDbContext_OptimizedQuery_Async()
        {
            var sw = new Stopwatch();

            sw.Start();
            var res = DoAsync(100000).Result;
            sw.Stop();


            Assert.True(sw.ElapsedMilliseconds < 300, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        //[Fact]
        public void TestDbContext_OptimizedQuery_Sync()
        {
            var sw = new Stopwatch();

            sw.Start();
            var res = DoSync(100000);
            sw.Stop();


            Assert.True(sw.ElapsedMilliseconds < 300, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        //[Fact]
        public void Connection_Time()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var sqlConnection = GetConnection())
            {
                sqlConnection.Open();
            }
            var e1 = sw.ElapsedMilliseconds;

            using (var sqlConnection = GetConnection())
            {
                sqlConnection.Open();
            }
            var e2 = sw.ElapsedMilliseconds -e1;

            using (var sqlConnection = GetConnection())
            {
                sqlConnection.Open();
            }
            var e3 = sw.ElapsedMilliseconds - e2 - e1;


            sw.Stop();
            Assert.True(e1 > (e2 + e3) * 10, $"{e1}:{e2}:{e3}");
        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection("");
        }

        //[Fact]
        public void DynamicLinq_Tests()
        {
            var lp = new InMemoryLinqProvider(new[]
            {
                new Product() {Name = "123", Id = 1}
                , new Product() {Name = "234"}
            });

            var val = lp.Query<Product>().Where("Name = \"123\"").ToArray();

            var autoFilter = new AutoFilter<Product>();
            autoFilter.Filter.Add("Name", "123");
            autoFilter.Filter.Add("Id", 1);

            var q = autoFilter.Apply(lp.Query<Product>());

            var res = q.ToArray();
        }

        //[Fact]
        public void WriteCommand()
        {
            var optimizedHandler = new OptimizedCommnadHadler();
            var sw = new Stopwatch();
            sw.Start();
            var specs = new List<CreateProductDto>();
            for (var i = 0; i < 50000; i++)
            {
                specs.Add(new CreateProductDto() {CategoryId = 1, Name = i.ToString(), Price = i });
            }

            var res = Task.WhenAll(specs.Select(x => optimizedHandler.Handle(x)).ToArray()).Result;
            sw.Stop();


            Assert.True(sw.ElapsedMilliseconds < 300, $"Elapsed Miliseconds: {sw.ElapsedMilliseconds}");
        }

        private static IPagedEnumerable<ProductDto>[] DoSync(int queryCount)
        {
            var optimizedQuery = new OptimizedQuery();
            var specs = new List<UberProductSpec>();
            for (var i = 0; i < queryCount; i++)
            {
                specs.Add(new UberProductSpec(1, 1) {Price = i});
            }

            var res = specs
                .Select(x => optimizedQuery.AskSync(x))
                .ToArray();

            return res;
        }

        private static async Task<IPagedEnumerable<ProductDto>[]> DoAsync(int queryCount)
        {
            var optimizedQuery = new OptimizedQuery();
            var specs = new List<UberProductSpec>();
            for (var i = 0; i < queryCount; i++)
            {
                specs.Add(new UberProductSpec(1, 1) {Price = i});
            }

            var res = await Task.WhenAll(specs
                .Select(x => optimizedQuery.Ask(x))
                .ToArray());

            //var res = specs.Select(x => optimizedQUery.Ask(x)).ToArray();

            return res;
        }
    }
}
