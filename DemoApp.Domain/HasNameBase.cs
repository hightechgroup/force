using System.ComponentModel.DataAnnotations;

namespace DemoApp.Domain
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
        public string Name { get; set; }
    }
}