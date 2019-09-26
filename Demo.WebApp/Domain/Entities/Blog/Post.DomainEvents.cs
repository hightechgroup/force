using System.Collections.Generic;
using Demo.WebApp.Features.Blog;
using Force.Ddd.DomainEvents;

namespace Demo.WebApp.Domain.Entities.Blog
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