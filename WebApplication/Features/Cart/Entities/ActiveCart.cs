using System;
using System.Collections.Generic;

namespace WebApplication.Features.Cart.Entities
{
    public class ActiveCart: Cart
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public OrderedCart Order()
        {
            throw new NotImplementedException();
        }

        public ActiveCart() : base(CartState.Active)
        {
        }
        
        public void Add(Category.Product product)
        {
            CartItems.Add(new CartItem(this, product));
        }

        public override string ToString()
            => base.ToString() + $". Product Count: {CartItems.Count}";
    }
}