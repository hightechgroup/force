using System;

namespace Demo.WebApp.Domain
{
    public class CommentEventBase: DomainEventBase
    {
        protected CommentEventBase()
        {            
        }
        
        public CommentEventBase(Comment comment)
        {
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public virtual Comment Comment { get; protected set; }
    }
}