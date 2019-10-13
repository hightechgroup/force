using System;
using Force.Ddd;

namespace WebApplication.Features.Cart.Entities
{
    public enum CartState
    {
        Active, // что-то есть в корзине
        Ordered, // выбран адрес доставки или самовывоз. Если доставка не курьером, то оплачен
        Shipped, // Tracking Code или назначен курьер
        Completed, // Доставлено, доки подписаны
        Disputed // Один из комментариев заполнен
    }
    
    public abstract class Cart: HasIdBase
    {
       public CartState State { get; protected set; }
       
       protected Cart(CartState state)
       {
           State = state;
       }

       public override string ToString()
           => $"Cart {Id} {State}";
    }
}