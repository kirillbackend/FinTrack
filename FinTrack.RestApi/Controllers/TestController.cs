using Microsoft.AspNetCore.Mvc;

namespace FinTrack.RestApi.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
