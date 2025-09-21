using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
