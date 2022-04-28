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
            return View(company);
        }








        [HttpGet]
        [Authorize]
        public IActionResult User()
        {
            var userName = HttpContext.User.Identity.Name;
            User user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == userName);

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

        [Authorize]
        public IActionResult Buy(int? id)
		{
            if (id == null) return RedirectToAction("Index", "Home");
			var tour = db.Tours.FirstOrDefault(t => t.Id == id);

			var userName = HttpContext.User.Identity.Name;
			User user = db.Users.FirstOrDefault(u => u.Email == userName);

			user?.Tours?.Add(tour);
			db.SaveChanges();
			return RedirectToAction("Tour", "Home", new { id = id });
        }





        private async Task Authenticate(string userName)
        {
            User user = db.Users.Include(u => u.Role). FirstOrDefault(u => u.Email == userName);
            if (user is null) return;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        // Возвращает текущего пользователя если его роль совпадает с переданной, вернет null если роль не совпадает.
        private User GetCurrentUserByRole(string role)
		{
            var userName = HttpContext.User.Identity.Name;
            User user = db.Users.Include(u => u.Role).Include(u => u.Tours)
                .ThenInclude(t => t.City).FirstOrDefault(u => u.Email == userName);
            if (user == null || user.Role.Name != role) return null;
            else return user;
        }
    }
}
