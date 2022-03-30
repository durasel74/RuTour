using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
			return View(db);
		}

		[HttpGet]
		public IActionResult Tour(int? id)
		{
			if (id == null) return RedirectToAction("Index");
			var tour = db.Tours.FirstOrDefault(tour => tour.Id == id);
			return View(tour);
		}

		[HttpGet]
		public IActionResult Company(int? id)
		{
			if (id == null) return RedirectToAction("Index");
			var compnay = db.Companies.FirstOrDefault(compnay => compnay.Id == id);
			return View(compnay);
		}

		

		//[HttpGet]
		//public IActionResult Users()
		//{
		//    return View(db.Users);
		//}

		//[HttpGet]
		//public IActionResult Users(int? id)
		//{
		//    if (id == null) return RedirectToAction("Index");
		//    ViewBag.UserId = id;
		//    return View(db.Users);
		//}



		//[HttpGet]
		//public IActionResult Country(int? id)
		//{
		//    if (id == null) return RedirectToAction("Index");
		//    ViewBag.CountryId = id;
		//    return View(db.Cities.Where(city => city.CountryId == id).ToList());
		//}

		//[HttpPost]
		//public string Country(Country country)
		//{
		//    return "Выбран город: " + country.Name;
		//}
	}
}
