using System;

namespace WebApplication.Features.Cart.Entities
{
    public class OrderedCart: Cart
    {
        public decimal Total { get; set; }

        public ShippedState Ship()
        {
            throw new NotImplementedException();
        }

        public OrderedCart() : base(CartState.Ordered)
        {
        }

    }
}