using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Services;

namespace WebApplication.Features.Account
{
    public class AccountController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> UpdatePassport(
            [FromServices] FileUploadService<UpdatePassport> fileUploadService,
            UpdatePassport command)
        {
            await fileUploadService.InTransactionAsync(command);
            return RedirectToAction("Index");
        }
    }
}