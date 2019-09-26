using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain.Entities.Account;
using Force.Ddd;

namespace Demo.WebApp.Features.Blog
{
    public class AddComment
    {       
        public Id<User> UserId { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}