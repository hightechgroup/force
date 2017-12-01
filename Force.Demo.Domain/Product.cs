using System;
using Force.Ddd;

namespace Force.Demo.Domain
{
    public class Product: HasIdBase<int>
    {
        public decimal Price { get; protected set; }

        public Product(decimal price)
        {
            Price = price;
        }
    }
}