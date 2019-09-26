using System;
using Demo.WebApp.Domain.Entities.Core;

namespace Demo.WebApp.Domain.Entities.Blog
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