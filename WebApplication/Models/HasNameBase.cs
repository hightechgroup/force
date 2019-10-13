using Force.Ddd;

namespace WebApplication.Models
{
    public class HasNameBase: HasIdBase
    {
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}