using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Services;

namespace WebApplication.Features.Account
{
    public class AccountController: Controller
    {
        public IActionResult Add(
            [FromServices] ApplicationDbContext dbContext)
        {
            if (Contact.TryParsePhone("+123", out var c))
            {
                var user = new User(c, new UserName("1", "2", "3"));
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        
        public IActionResult Index(
            [FromServices] ApplicationDbContext dbContext
            )
        {
            var data = dbContext
                .Users
                .Where(x => x.UserName.FirstName == "1")
                .Select(x => new
                {
                    x.UserName.FirstName,
                    x.Contact.Email
                })
                .ToList();
            
            return View(data);
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