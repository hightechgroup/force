using Microsoft.AspNetCore.Mvc;
using WebApplication.Features;
using WebApplication.Features.Category;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ControllerBase: Controller
    {
        public IActionResult Index()
        {
            return View(new Product[]{new Product(){Name = "123"}});
        }

        public IActionResult Display(int id)
        {
            return View(id);
        }
    }
}