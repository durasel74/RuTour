using Microsoft.AspNetCore.Mvc;

namespace RuTour.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
