using Microsoft.AspNetCore.Http;

namespace WebApplication.Services
{
    public interface IHasFile
    {
        IFormFile FormFile { get; }
    }
}