using Force.Ddd;

namespace Force.Demo.Domain
{
    public class Category: HasIdBase<int>
    {
        public static class Specs
        {
            public static Spec<Category> IsActive => new Spec<Category>(x => x.IsActive);
        }

        public Category(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public bool IsActive { get; set; }
        
        public string Name { get; protected set; }
        
        public string Url { get; protected set; }
    }
}