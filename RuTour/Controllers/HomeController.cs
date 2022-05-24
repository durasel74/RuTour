using System.Text.RegularExpressions;
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
		public IActionResult Index(string? title_frag, string? city, string? company, DateTime? date, 
			int? nights, string? transport, string? return_, string? cost, int? max_cost)
		{
			db.ClearSearchList();
			if (!String.IsNullOrEmpty(title_frag))
				db.SearchList = db.SearchList.Where(t => t.Title.ToLower().StartsWith(title_frag.ToLower())).ToList();
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
			if (cost != null)
			{
				string[] splitedCost = cost.Split('-');
				int minCost = int.Parse(Regex.Replace(splitedCost[0], @"[^\d]+", ""));
				int maxCost = int.Parse(Regex.Replace(splitedCost[1], @"[^\d]+", ""));
				db.SearchList = db.SearchList.Where(t => t.Cost >= minCost && t.Cost <= maxCost).ToList();
			}
			return View(db);
		}

		[HttpGet]
		public IActionResult Tour(int? id)
		{
			if (id == null) return RedirectToAction("Index");
			var tour = db.Tours.Include(t => t.Company).Include(t => t.City).Include(t => t.Accommodation)
				.Include(t => t.Claimes).ThenInclude(u => u.User).FirstOrDefault(tour => tour.Id == id);
			ViewBag.Role = GetRole();
			ViewBag.CompanyLogin = HttpContext.User.Identity.Name;

			var user = db.Users.FirstOrDefault(user => user.Email == HttpContext.User.Identity.Name);
			if (user != null)
			{
				Models.Claim claim = db.Claimes.Include(c => c.Tour).Include(c => c.User)
					.FirstOrDefault(c => c.Tour.Id == tour.Id && c.User.Id == user.Id);
				ViewBag.Claim = claim;
			}
			return View(tour);
		}

		[HttpGet]
		public IActionResult Company(int? id)
		{
			if (id == null) return RedirectToAction("Index");
			var company = db.Companies.Include(c => c.Tours).ThenInclude(t => t.City).FirstOrDefault(compnay => compnay.Id == id);
			return View(company);
		}

		[HttpGet]
		public IActionResult About()
		{
			return View();
		}

		public string GetRole()
		{
			if (HttpContext.User.IsInRole("admin"))
				return "admin";
			else if (HttpContext.User.IsInRole("user"))
				return "user";
			else if (HttpContext.User.IsInRole("company"))
				return "company";
			else return "";
		}
	}
}
