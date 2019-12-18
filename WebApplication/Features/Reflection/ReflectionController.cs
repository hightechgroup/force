using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Features.Reflection
{
    public class ReflectionController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}