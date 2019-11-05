using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Features.Cart.Entities
{
    public class ActiveCart: Cart
    {
        private List<CartItem> _cartItems = new List<CartItem>();

        public IReadOnlyList<CartItem> CartItems => _cartItems;
        
        public void Add(Product product)
        {
            _cartItems.Add(new CartItem(this, product));
        }
        
        public void Clear()
        {
            _cartItems.Clear();
        }

        public OrderedCart Order()
        {
            var ordered = new OrderedCart()
            {
                Id = Id
            };

            return ordered;
        }

        public ActiveCart() : base(CartState.Active)
        {
        }

        public override string ToString()
            => base.ToString() + $". Product Count: {CartItems.Count}";
    }
}