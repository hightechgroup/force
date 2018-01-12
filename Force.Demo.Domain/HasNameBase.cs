using System.ComponentModel.DataAnnotations;
using LinqToDB.Mapping;

namespace Force.Demo.Domain
{
    public abstract class HasNameBase : EntityBase<int>
    {
        protected HasNameBase()
        {            
        }
        
        protected HasNameBase(string name)
        {
            Name = name;
        }

        [Required, DisplayFormat(ConvertEmptyStringToNull = true)]
        [Column(Name = "Name"), NotNull]
        public string Name { get; set; }
    }
}