using System;
using Demo.WebApp.Domain.Entities.Blog;
using Demo.WebApp.Domain.Entities.Core;

namespace Demo.WebApp.Features.Blog
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