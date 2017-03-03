using System;
using System.Linq;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;
using Microsoft.EntityFrameworkCore;

namespace CostEffectiveCode.Tests.Stubs
{
    public class TestDbContext : DbContext, ILinqProvider
    {
        public TestDbContext()
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class, IHasId
        {
            return Set<TEntity>();
        }

        public IQueryable Query(Type t)
        {
            throw new NotImplementedException();
        }
    }
}
