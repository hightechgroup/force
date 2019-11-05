using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Features.Account;
using WebApplication.Features.Cart.Entities;

namespace WebApplication.Data
{
    public class ApplicationDbContext : DbContext//IdentityDbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Features.Account.User> Users { get; set; }

        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<ActiveCart> ActiveCarts { get; set; }
        
        public DbSet<OrderedCart> OrderedCarts { get; set; }
        
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
    
            builder.Entity<User>(ob =>
                {
                    ob
                        .HasOne(o => o.Contact)
                        .WithOne()
                        .HasForeignKey<Contact>(o => o.Id);
                    
                    ob
                        .HasOne(o => o.UserName)
                        .WithOne()
                        .HasForeignKey<UserName>(o => o.Id);
                }
            );

            builder.Entity<Contact>(ob => ob.ToTable("Users")); 
            builder.Entity<UserName>(ob => ob.ToTable("Users"));
            
//            builder.Entity<ActiveCart>()
//                .Property(b => b.CartItems)
//                .HasField("_cartItems")
//                .UsePropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Entity<Cart>().HasQueryFilter(
                p => p is ActiveCart 
                    ? p.State == CartState.Active
                    // ReSharper disable once SimplifyConditionalTernaryExpression
                    : p is OrderedCart
                    ? p.State == CartState.Ordered
                    : true  
            );
        }
    }
}