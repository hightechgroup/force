using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Features.Cart.Entities;

namespace WebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<ActiveCart> ActiveCarts { get; set; }
        
        public DbSet<OrderedCart> OrderedCarts { get; set; }
        
        public DbSet<CartItem> CartItems { get; set; }
    }
}