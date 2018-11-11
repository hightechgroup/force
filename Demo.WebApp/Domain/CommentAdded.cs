using System;

namespace Demo.WebApp.Domain
{
    public class CommentAdded: DomainEventBase
    {
        public CommentAdded(Comment comment)
        {
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public Comment Comment { get; protected set; }
    }
}