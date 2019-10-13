using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WebApplication.Services;

namespace WebApplication.Features.Account
{
    public class UpdatePassport: IHasFile
    {
        [Required]
        public IFormFile? FormFile { get; set; }
        
        [Required]
        public string? Number { get; set; }
    }
}