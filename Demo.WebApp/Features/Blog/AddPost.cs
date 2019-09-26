using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain.Entities.Account;
using Demo.WebApp.Domain.Entities.Blog;
using Demo.WebApp.Domain.Entities.Core;
using Force.Ddd;

namespace Demo.WebApp.Features.Blog
{
    public class AddPost
    {
        [Required]
        public Id<Hub> Hub { get; set; }
        
        [Required]
        public Id<User> User { get; set; }

        [Required]
        [DefaultStringLength]
        public string Name { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}