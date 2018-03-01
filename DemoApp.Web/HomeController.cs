using Microsoft.AspNetCore.Mvc;

namespace Force.Demo.Web
{
    // permanent redirect 302
    // staff only 404?
    // model binding 404, 415, 422
    // validation 404, 415 422 Result<T, ValidationFailure>
    // security 401, 403 Result<T, SecurityFailure>
    // business logic validation 422 Result<T, Failure>
    // проверка на 15% (фгис), закрытый 
    // response data 200 / tmp redirect 301
    
    public class HomeController: Controller
    {
        public IActionResult Index() => View();
    }
}