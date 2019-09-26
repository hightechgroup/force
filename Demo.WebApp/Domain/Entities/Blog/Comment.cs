using System;
using System.Collections.Generic;
using Demo.WebApp.Domain.Entities.Account;
using Demo.WebApp.Domain.Entities.Core;
using Demo.WebApp.Features.Blog;
using Force.Ddd.DomainEvents;

namespace Demo.WebApp.Domain.Entities.Blog
{
    public class Comment
        : EntityBase
        , IHasDomainEvents
        , IHasText
    {
        private DomainEventStore _domainEventStore = new DomainEventStore();
        
        protected Comment()
        {}
        
        public Comment(Post post, User user, string text, Comment parent = null)
        {
            Post = post ?? throw new ArgumentNullException(nameof(post));
            User = user ?? throw new ArgumentNullException(nameof(user));
            Text = text;
            Parent = parent;
        }
        
        public virtual Post Post { get; protected set; }
        
        public virtual User User { get; protected set; }
        
        public virtual Comment Parent { get; protected set; }
        
        public string Text { get; protected set; }

        public void Update(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            _domainEventStore.Raise(new CommentUpdated(this));
        }

        public IEnumerable<IDomainEvent> GetDomainEvents() => _domainEventStore;
    }
}