using System.ComponentModel.DataAnnotations;

namespace Force.Ddd
{
    public abstract class HasNameBase : HasIdBase
    {
        protected HasNameBase() {}

        protected HasNameBase(string name)
        {
            Name = name;
        }

        [Required, MinLength(1), MaxLength(Strings.DefaultLength)]
        public virtual string Name { get; protected set; }
    }
}