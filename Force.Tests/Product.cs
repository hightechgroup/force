namespace Force.Tests
{
    public class Product
    {
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}