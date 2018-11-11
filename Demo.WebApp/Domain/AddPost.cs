using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Demo.WebApp.Domain
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