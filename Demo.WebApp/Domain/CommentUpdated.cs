namespace Demo.WebApp.Domain
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