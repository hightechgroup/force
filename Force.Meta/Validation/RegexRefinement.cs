using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Force.Meta.Validation
{
    public class RegexRefinement<T> : Refinement<T>
    {
        public RegexRefinement(string pattern, string errorMessage) 
            : base(BuildRegexExpression(pattern), errorMessage)
        {
        }

        public static Expression<Func<T, bool>> BuildRegexExpression(string pattern)
        {
            var regex = new Regex(pattern);
          
            return x => regex.Match(x.ToString()).Success;
        }
    }
}