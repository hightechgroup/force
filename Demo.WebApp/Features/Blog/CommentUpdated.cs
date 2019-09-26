using Demo.WebApp.Domain.Entities.Blog;

namespace Demo.WebApp.Features.Blog
{
    public class CommentUpdated: CommentEventBase
    {
        protected CommentUpdated()
        {            
        }
        
        public CommentUpdated(Comment comment) : base(comment)
        {
        }
    }
}