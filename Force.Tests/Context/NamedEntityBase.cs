namespace Force.Tests
{
    public abstract class NamedEntityBase
    {
        protected NamedEntityBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; protected set; }
        
        public string Name { get; protected set; }
    }
}