namespace Force.Tests
{
    public class SaleItem: NamedEntityBase
    {
        public Product Product { get; protected set; }

        public SaleItem(int id, string name, Product product) : base(id, name)
        {
            Product = product;
        }
    }
}