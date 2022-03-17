using Microsoft.AspNetCore.Mvc;
using RuTour.Models;

namespace RuTour.Controllers
{
	public class HomeController : Controller
	{
		TourContext db;
		public HomeController(TourContext context)
		{
			db = context;
		}

		public IActionResult Index()
		{
			return View(db.Countries.ToList());
		}

        [HttpGet]
        public IActionResult Users()
        {
            return View(db.Users);
        }

        //[HttpGet]
        //public IActionResult Users(int? id)
        //{
        //    if (id == null) return RedirectToAction("Index");
        //    ViewBag.UserId = id;
        //    return View(db.Users);
        //}

        [HttpGet]
        public IActionResult Tours(int? id)
        {
            if (id == null) return RedirectToAction("Home/Users/");
            ViewBag.TourId = id;
            var user = db.Users.FirstOrDefault(user => user.Id == id);
            return View(user);
        }

        [HttpGet]
        public IActionResult Country(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.CountryId = id;
            return View(db.Cities.Where(city => city.CountryId == id).ToList());
        }
        //[HttpPost]
        //public string Country(Country country)
        //{
        //    return "Выбран город: " + country.Name;
        //}
    }
}
