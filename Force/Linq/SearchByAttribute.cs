using System;

namespace Force.Linq
{
    public enum SearchKind
    {
        StartsWith,
        Contains            
    }
    
    public class SearchByAttribute : Attribute
    {
        public SearchByAttribute(object anywhere)
        {

        }
    }
}