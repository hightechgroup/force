using System;
using Demo.WebApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Demo.WebApp.Data
{
    public class DemoAppDbContext: DbContext
    {
        public DbSet<CommentUpdated> CommentUpdatedEvents { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Hub> Hubs { get; set; }
        
        public DbSet<Account> Accounts { get; set; }

        public DemoAppDbContext(DbContextOptions options) : base(options)
        {            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .ConfigureWarnings(w => w
                    .Log(CoreEventId.IncludeIgnoredWarning)
                    .Throw(RelationalEventId.QueryClientEvaluationWarning));
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This will singularize all table names
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var r = entityType.Relational();
                r.TableName = entityType.DisplayName();

                //Console.WriteLine(entityType.ClrType + " " + typeof(DomainEventBase).IsAssignableFrom(entityType.ClrType));
                if (typeof(DomainEventBase).IsAssignableFrom(entityType.ClrType))
                {
                    r.Schema = "events";
                }
            }

            var converter = new ValueConverter<Email, string>(
                v => v,
                v => new Email(v));

            modelBuilder
                .Ignore<Email>()
                .Ignore<CommentAdded>();

            modelBuilder
                .Entity<Account>()
                .Property(e => e.Email)
                .HasConversion(converter);

            var cu = modelBuilder.Entity<CommentUpdated>();

            cu.Property<int>("PostId");
            cu.HasKey("PostId");

            var hub = new Hub("DotNext Moscow 2018");
            hub.WithId(1);
            
            modelBuilder
                .Entity<User>()
                .HasData(new User(new Email("max@hightech.group"), "Max", "Arshinov").WithId(1));

            modelBuilder
                .Entity<Hub>()
                .HasData(hub);

//            modelBuilder
//                .Entity<Post>()
//                .HasData(
//                    new Post("Оптимизации внутри .NET Core", DefaultText, hub).WithId(1),
//                    new Post("Помочь всем человекам: зачем мы написали свой движок чат-бота", DefaultText, hub).WithId(2),
//                    new Post("So you want to create your own .NET runtime?", DefaultText, hub).WithId(3)
//                );
        }
    }
}