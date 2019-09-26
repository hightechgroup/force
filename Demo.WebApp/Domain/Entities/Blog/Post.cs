using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain.Entities.Core;
using Demo.WebApp.Features.Blog;

namespace Demo.WebApp.Domain.Entities.Blog
{
    public partial class Post
        : NamedEntityBase
        , IAuditable
        , IHasText
        , IPublishable
    {
        protected Post()
        {}
        
        public Post(string name, string text, string url, Hub hub) : base(name)
        {
            Text = string.IsNullOrEmpty(text)
                ? text
                : throw new ArgumentNullException(nameof(text));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }
        
        public virtual Hub Hub { get; protected set; }

        public virtual string Url { get; protected set; }

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