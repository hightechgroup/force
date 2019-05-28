using System.Collections;
using System.Collections.Generic;
using Force.Ddd;
using Force.Ddd.DomainEvents;

namespace Demo.WebApp.Domain
{
    public partial class Post: IHasDomainEvents
    {
        private readonly List<CommentAdded> _commentAddedEvents = new List<CommentAdded>();
        
        public IEnumerable<IDomainEvent> GetDomainEvents()
        {
            foreach (var commentAdded in _commentAddedEvents)
            {
                yield return commentAdded;
            }            
        }
    }
}