using Microsoft.EntityFrameworkCore;
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

		[HttpPost]
		public IActionResult Index(string? city, string? company, DateTime? date, 
			int? nights, string? transport, string? return_, int? max_cost)
		{
			db.ClearSearchList();
			if (!String.IsNullOrEmpty(city))
				db.SearchList = db.SearchList.Where(t => t.City.Name == city).ToList();
			if (!String.IsNullOrEmpty(company))
				db.SearchList = db.SearchList.Where(t => t.Company.Name == company).ToList();
			if (date != null)
				db.SearchList = db.SearchList.Where(t => t.Date == date).ToList();
			if (nights != null)
				db.SearchList = db.SearchList.Where(t => t.NightsCount == nights).ToList();
			if (!String.IsNullOrEmpty(transport))
				db.SearchList = db.SearchList.Where(t => t.TransportString == transport).ToList();
			if (!String.IsNullOrEmpty(return_))
				db.SearchList = db.SearchList.Where(t => t.ReturnString == return_).ToList();
			if (max_cost != null)
				db.SearchList = db.SearchList.Where(t => t.Cost <= max_cost).ToList();
			return View(db);
		}

		[HttpGet]
		public IActionResult Tour(int? id)
		{
			if (id == null) return RedirectToAction("Index");
			var tour = db.Tours.Include(t => t.Company).Include(t => t.City).FirstOrDefault(tour => tour.Id == id);
			return View(tour);
		}

		[HttpGet]
		public IActionResult Company(int? id)
		{
			if (id == null) return RedirectToAction("Index");
			var company = db.Companies.Include(c => c.Tours).ThenInclude(t => t.City).FirstOrDefault(compnay => compnay.Id == id);
			return View(company);
		}
	}
}
