using System;
using System.Collections;
using System.Collections.Generic;
using Force.Ddd;

namespace Demo.WebApp.Domain
{
    public class Comment
        : EntityBase
        , IHasDomainEvents
        , IHasText
    {
        protected Comment()
        {}
        
        public Comment(Post post, User user, string text, Comment parent = null)
        {
            Post = post ?? throw new ArgumentNullException(nameof(post));
            User = user ?? throw new ArgumentNullException(nameof(user));
            Text = text;
            Parent = parent;
        }
                
        private readonly List<CommentUpdated> _commentUpdatedEvents = new List<CommentUpdated>();
                
        public virtual Post Post { get; protected set; }
        
        public virtual User User { get; protected set; }
        
        public virtual Comment Parent { get; protected set; }
        
        public string Text { get; protected set; }

        public void Update(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            this.Raise(_commentUpdatedEvents, new CommentUpdated(this));
        }

        public IEnumerable<IDomainEvent> GetDomainEvents()
            => _commentUpdatedEvents;
    }
}