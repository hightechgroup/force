using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new Product[]{new Product(){Name = "123"}});
        }

        public IActionResult Display(int id)
        {
            return View(id);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}