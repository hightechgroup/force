using DemoApp.Domain;
using Force.Meta.Validation;

namespace DemoApp.Core
{
    public class User: HasNameBase
    {
        [Refinement(typeof(AdultRefinement))]
        public int Age { get; set; }
    }
}