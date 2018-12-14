namespace Demo.WebApp.Domain
{
    public class Hub: NamedEntityBase
    {
        public Hub(string name) : base(name)
        {
        }
        
        public string Url { get; set; }
    }
}