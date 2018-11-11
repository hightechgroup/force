using System.Collections;
using System.Collections.Generic;
using Force.Ddd;

namespace Demo.WebApp.Domain
{
    public partial class Post: IHasDomainEvents
    {
        private readonly List<CommentAdded> _commentAddedEvents = new List<CommentAdded>();
        
        public IEnumerable GetDomainEvents()
        {
            foreach (var commentAdded in _commentAddedEvents)
            {
                yield return commentAdded;
            }            
        }
    }
}