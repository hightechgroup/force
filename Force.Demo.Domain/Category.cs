using Force.Ddd;

namespace Force.Demo.Domain
{
    public class Category: HasNameBase
    {
        public static class Specs
        {
            //public static Spec<Category> IsActive => new Spec<Category>(x => x.IsActive);
        }

        protected Category()
        {            
        }

        public Category(string name, string url):base(name)
        {
            Url = url;
        }

        public bool IsActive { get; set; }

        public string Url { get; protected set; }
    }
}