using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Force.Extensions;

namespace Demo.WebApp.Domain
{
    public partial class Post
        : NamedEntityBase
        , IAuditable
        , IHasText
        , IPublishable
    {
        protected Post()
        {}
        
        public Post(string name, string text, Hub hub) : base(name)
        {
            Text = text.NullIfEmpty() ?? throw new ArgumentNullException(nameof(text));
            Hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }
        
        public virtual Hub Hub { get; protected set; }

        [Required]
        public string Text { get; set; }
        
        private readonly List<Comment> _comments = new List<Comment>();

        private IEnumerable<Comment> Comments => _comments;        

        public void AddComment(AddComment command)
        {
            var comment = new Comment(this, command.UserId, command.Text);
            _comments.Add(comment);
            //this.RaiseDomainEvent(_commentAddedEvents, new CommentAdded(comment));
            LastUpdated = DateTime.UtcNow;
        }

        public DateTime Created { get; protected set; } = DateTime.UtcNow;
        
        public DateTime? LastUpdated { get; protected set;}
        
        public DateTime? Published { get; protected set; }
        
        public void Publish()
        {
            Published = DateTime.UtcNow;
        }
    }
}