using System.ComponentModel.DataAnnotations;

namespace DotNext.DddWorkshop.Models
{
    public class HasNameBase: HasIdBase
    {
        [Required]
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}