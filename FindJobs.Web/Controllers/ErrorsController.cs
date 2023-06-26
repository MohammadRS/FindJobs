using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FindJobs.Web.Controllers
{
    public class ErrorsController : BaseController
    {
        public ErrorsController(IConfiguration configuration) : base(configuration)
        {

        }
        public IActionResult Page404()
        {
            return View();
        }
    }
}
