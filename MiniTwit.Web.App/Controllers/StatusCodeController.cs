using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MiniTwit.Web.App.Controllers
{
    public class StatusCodeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public StatusCodeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Status404()
        {
            _logger.LogInformation("Returned a 404");
            return View();
        }
    }
}