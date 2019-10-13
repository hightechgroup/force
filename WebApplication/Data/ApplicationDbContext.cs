using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Features.Cart.Entities;

namespace WebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<ActiveCart> ActiveCarts { get; set; }
        
        public DbSet<OrderedCart> OrderedCarts { get; set; }
        
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Cart>().HasQueryFilter(
                p => p is ActiveCart 
                    ? p.State == CartState.Active
                    : p is OrderedCart
                    ? p.State == CartState.Ordered
                    : true  
            );
        }
    }
}