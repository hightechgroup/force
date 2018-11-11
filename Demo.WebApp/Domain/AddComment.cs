using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Demo.WebApp.Domain
{
    public class AddComment
    {       
        public Id<User> UserId { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}