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
	public class AccountController : Controller
	{
        private TourContext db;
        public AccountController(TourContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    var userRole = db.Roles.First(r => r.Name == "user");
					db.Users.Add(new User
					{
						Email = model.Email,
						Password = model.Password,
						Role = userRole
					});
					await db.SaveChangesAsync();

                    // await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Login", "Account");
                }
                else ModelState.AddModelError("", "Пользователь с такой почтой уже существует");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
					// добавляем пользователя в бд
					var companyRole = db.Roles.First(r => r.Name == "company");
                    var newUser = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        Role = companyRole
                    };
                    db.Users.Add(newUser);

					// добавляем компанию в бд
                    db.Companies.Add(new Company
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Description = ""
                    });
                    await db.SaveChangesAsync();

                    // await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Login", "Account");
                }
                else ModelState.AddModelError("", "Пользователь с такой почтой уже существует");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult User()
        {
            var userName = HttpContext.User.Identity.Name;
            User user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == userName);

            if (user == null) return RedirectToAction("Login", "Account");
            switch (user.Role.Name)
            {
                case "admin":
                    return RedirectToAction("AdminAccount", "Account");
                case "user":
                    return RedirectToAction("UserAccount", "Account");
                case "company":
                    return RedirectToAction("CompanyAccount", "Account");
                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AdminAccount()
        {
            User user = GetCurrentUserByRole("admin");
            if (user == null) return RedirectToAction("Login", "Account");
            user.DB = db;
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public IActionResult UserAccount()
		{
            User user = GetCurrentUserByRole("user");
            if (user == null) return RedirectToAction("Login", "Account");
            user.DB = db;
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "company")]
        public IActionResult CompanyAccount()
        {
            User user = GetCurrentUserByRole("company");
            if (user == null) return RedirectToAction("Login", "Account");
            var userName = HttpContext.User.Identity.Name;
            Company company = db.Companies.Include(c => c.Tours).First(u => u.Email == userName);
            company.DB = db;
            return View(company);
        }

        [HttpPost]
        [Authorize(Roles = "company")]
        public IActionResult AddTour(string? tour_title, string? city,
            string? accomodation, DateTime? date, decimal? cost, int? nights_count,
            int? tickets_count, string? transport, string? return_, string? description_field)
        {
            if (tour_title == null || city == null || date == null || cost == null || nights_count == null
                || tickets_count == null || transport == null || return_ == null)
                return RedirectToAction("User", "Account");

            var userName = HttpContext.User.Identity.Name;
            Company company = db.Companies.First(u => u.Email == userName);

            var tour = db.Tours.FirstOrDefault(t => t.Title == tour_title && t.Company.Name == company.Name);
            if (tour == null)
            {
                tour = new Tour
                {
                    Title = tour_title,
                    Company = company,
                    City = db.Cities.First(c => c.Name == city),
                    Accommodation = db.Accommodations.FirstOrDefault(a => a.Name == accomodation),
                    Date = date.GetValueOrDefault(),
                    Cost = cost.GetValueOrDefault(),
                    NightsCount = nights_count.GetValueOrDefault(),
                    MaxTicketNumber = tickets_count.GetValueOrDefault(),
                    Transport = Transport.None.ToTransport(transport),
                    Return = return_ == "Есть",
                    Description = description_field ?? "",
                };
                db.Tours.Add(tour);
                db.SaveChanges();
            }
            return RedirectToAction("CompanyAccount", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "company")]
        public IActionResult DeleteTour(int? id)
        {
            Tour tour = db.Tours.FirstOrDefault(t => t.Id == id);
            if (tour != null)
            {
                db.Tours.Remove(tour);
                db.SaveChanges();
            }
            return RedirectToAction("CompanyAccount", "Account");
        }

		[Authorize(Roles = "user")]
		public IActionResult ClimeTour(int? id, int? count)
		{
			if (id == null || count == null) return RedirectToAction("Index", "Home");
			var tour = db.Tours.FirstOrDefault(t => t.Id == id);
			var userName = HttpContext.User.Identity.Name;
			User user = db.Users.FirstOrDefault(u => u.Email == userName);

            var newClaim = new Models.Claim {
                Tour = tour,
                User = user,
                Count = count.Value,
                Accepted = false
            };
			user?.Claimes?.Add(newClaim);
			db.SaveChanges();
			return RedirectToAction("Tour", "Home", new { id = id });
		}

		[HttpPost]
		[Authorize(Roles = "company")]
		public IActionResult AcceptUserClime(int? tour_id, int? user_id)
		{
			Models.Claim claim = db.Claimes.Include(c => c.Tour).Include(c => c.User)
				.FirstOrDefault(c => c.Tour.Id == tour_id && c.User.Id == user_id);
			if (claim != null)
			{
                claim.Accepted = true;
				db.SaveChanges();
			}
			return RedirectToAction("Tour", "Home", new { id = tour_id });
		}

		[HttpPost]
		[Authorize(Roles = "company")]
		public IActionResult DeacceptUserClime(int? tour_id, int? user_id)
		{
			Models.Claim claim = db.Claimes.Include(c => c.Tour).Include(c => c.User)
				.FirstOrDefault(c => c.Tour.Id == tour_id && c.User.Id == user_id);
			if (claim != null)
			{
				claim.Accepted = false;
				db.SaveChanges();
			}
			return RedirectToAction("Tour", "Home", new { id = tour_id });
		}

		[HttpPost]
		[Authorize(Roles = "company")]
		public IActionResult DeleteUserClime(int? tour_id, int? user_id)
		{
			Models.Claim claim = db.Claimes.Include(c => c.Tour).Include(c => c.User)
				.FirstOrDefault(c => c.Tour.Id == tour_id && c.User.Id == user_id);
			if (claim != null)
			{
				db.Claimes.Remove(claim);
				db.SaveChanges();
			}
			return RedirectToAction("Tour", "Home", new { id = tour_id });
		}

		private async Task Authenticate(string userName)
        {
            User user = db.Users.Include(u => u.Role). FirstOrDefault(u => u.Email == userName);
            if (user is null) return;

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new System.Security.Claims.Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        // Возвращает текущего пользователя если его роль совпадает с переданной, вернет null если роль не совпадает.
        private User GetCurrentUserByRole(string role)
		{
            var userName = HttpContext.User.Identity.Name;
            User user = db.Users.Include(u => u.Role).Include(u => u.Claimes).ThenInclude(c => c.Tour)
                .ThenInclude(t => t.City).FirstOrDefault(u => u.Email == userName);
            if (user == null || user.Role.Name != role) return null;
            else return user;
        }
    }
}
