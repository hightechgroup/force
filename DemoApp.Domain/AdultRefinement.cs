using Force.Meta.Validation;

namespace DemoApp.Domain
{
    public class AdultRefinement: Refinement<int>
    {
        public AdultRefinement() : 
            base(x => x >= 18, "For adults only")
        {
        }
    }
}