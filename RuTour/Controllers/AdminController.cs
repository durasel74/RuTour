using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using RuTour.ViewModels;
using RuTour.Models;

namespace RuTour.Controllers
{
	public class AdminController : Controller
	{
		private TourContext db;
		public AdminController(TourContext context)
		{
			db = context;
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public IActionResult AddCity(string? city_name, string? country_name)
		{
			if (city_name == null || country_name == null)
				return RedirectToAction("User", "Account");

			var city = db.Cities.FirstOrDefault(c => c.Name == city_name);
			if (city == null)
			{
				city = new City
				{
					Name = city_name,
					Country = db.Countries.First(c => c.Name == country_name),
				};
				db.Cities.Add(city);
				db.SaveChanges();
			}
			return RedirectToAction("User", "Account");
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public IActionResult AddAccomodation(string? accomodation_name, string? accomodation_address,
			string? accomodation_description, string? city)
		{
			if (accomodation_name == null || accomodation_address == null || city == null)
				return RedirectToAction("User", "Account");

			var accommodation = db.Accommodations.FirstOrDefault(c => c.Name == accomodation_name);
			if (accommodation == null)
			{
				accommodation = new Accommodation
				{
					Name = accomodation_name,
					Description = accomodation_description ?? "",
					Address = accomodation_address,
					City = db.Cities.First(c => c.Name == city),
				};
				db.Accommodations.Add(accommodation);
				db.SaveChanges();
			}
			return RedirectToAction("User", "Account");
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public IActionResult AddCompany(string? company_email, string? company_phonenumber, 
		string? company_name, string? company_description)
		{
			if (company_email == null || company_name == null)
				return RedirectToAction("User", "Account");

			var company = db.Companies.FirstOrDefault(c => c.Email == company_email || c.Name == company_name);
			if (company == null)
			{
				company = new Company
				{
					Email = company_email,
					PhoneNumber = company_phonenumber ?? "",
					Name = company_name,
					Description = company_description ?? "",
				};
				db.Companies.Add(company);
				db.SaveChanges();
			}
			return RedirectToAction("User", "Account");
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public IActionResult AddTour(string? tour_title, string? company, string? city,
			string? accomodation, DateTime? date, decimal? cost, int? nights_count,
			int? tickets_count, string? transport, string? return_, string? tour_description)
		{
			if (tour_title == null || company == null || city == null
				|| date == null || cost == null || nights_count == null || tickets_count == null
				|| transport == null || return_ == null)
				return RedirectToAction("User", "Account");

			var tour = db.Tours.FirstOrDefault(t => t.Title == tour_title && t.Company.Name == company);
			if (tour == null)
			{
				tour = new Tour
				{
					Title = tour_title,
					Company = db.Companies.First(c => c.Name == company),
					City = db.Cities.First(c => c.Name == city),
					Accommodation = db.Accommodations.FirstOrDefault(a => a.Name == accomodation),
					Date = date.GetValueOrDefault(),
					Cost = cost.GetValueOrDefault(),
					NightsCount = nights_count.GetValueOrDefault(),
					MaxTicketNumber = tickets_count.GetValueOrDefault(),
					Transport = Transport.None.ToTransport(transport),
					Return = return_ == "Есть",
					Description = tour_description ?? "",
				};
				db.Tours.Add(tour);
				db.SaveChanges();
			}
			return RedirectToAction("User", "Account");
		}
	}
}
