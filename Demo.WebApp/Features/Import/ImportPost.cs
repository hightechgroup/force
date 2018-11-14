using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Domain;
using Force.Ddd;

namespace Demo.WebApp.Features.Posts
{
    public class ImportPost
    {
        [Required]
        public string HubName { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }
        
        public IEnumerable<CommentDto> Comments { get; set; }
    }

    public class CommentDto
    {
        [Required]
        public Email Email { get; set; }
        
        public IEnumerable<CommentDto> Comments { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}