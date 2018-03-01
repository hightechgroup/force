using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace DemoApp.Domain
{
    public abstract class HasNameBase : HasIdBase<int>
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