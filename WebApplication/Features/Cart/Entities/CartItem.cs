using Force.Ddd;

namespace WebApplication.Features.Cart.Entities
{
    public class CartItem: HasIdBase
    {
        protected CartItem()
        {
        }
        
        public CartItem(ActiveCart cart, Product product)
        {
            Cart = cart;
            Product = product;
        }
        
        public ActiveCart Cart { get; set; }
        
        public Product Product { get; set; }
    }
}