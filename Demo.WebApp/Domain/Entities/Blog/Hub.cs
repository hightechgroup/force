using Demo.WebApp.Domain.Entities.Core;

namespace Demo.WebApp.Domain.Entities.Blog
{
    public class Hub: NamedEntityBase
    {
        public Hub(string name) : base(name)
        {
        }
        
        public string Url { get; set; }
    }
}