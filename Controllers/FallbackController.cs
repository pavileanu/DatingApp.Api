using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MyApi.Controllers
{
    // [Route("/")]
    public class FallbackController : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }
    }
}