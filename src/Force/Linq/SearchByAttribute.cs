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
        public SearchKind SearchKind { get; }

        public SearchByAttribute(SearchKind searchKind = SearchKind.StartsWith)
        {
            SearchKind = searchKind;
        }
        
    }
}