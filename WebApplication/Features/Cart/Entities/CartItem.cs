using Force.Ddd;

namespace WebApplication.Features.Cart.Entities
{
    public class CartItem: HasIdBase
    {
        protected CartItem()
        {
        }
        
        public CartItem(Cart cart, Category.Product product)
        {
            Cart = cart;
            Product = product;
        }
        
        public Cart Cart { get; set; }
        
        public Category.Product Product { get; set; }
    }
}