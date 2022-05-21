using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RuTour.Models
{
	public class TourContext : DbContext
	{
		private List<String> transports = new List<String>();
		private List<Tour> searchList = new List<Tour>();

		public DbSet<Country> Countries { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Accommodation> Accommodations { get; set; }
		public DbSet<Tour> Tours { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Claim> Claimes { get; set; }

		public List<String> Transports
		{
			get
			{
				if (transports.Count == 0)
				{
					foreach (Transport transport in Enum.GetValues(typeof(Transport)))
						transports.Add(transport.ToStringRu());
				}
				return transports;
			}
		}

		public List<Tour> SearchList { get; set; }

		public TourContext(DbContextOptions<TourContext> options)
			: base(options)
		{
			// Database.EnsureDeleted();
			Database.EnsureCreated();

			if (!this.Countries.Any()) AddDefaultCountries();
			if (!this.Roles.Any()) AddRoles();
			if (!this.Users.Any()) AddAdmin();
			if (!this.Cities.Any()) AddDefaultCities();

			//if (!this.Countries.Any() && !this.Cities.Any()
			//	&& !this.Companies.Any() && !this.Accommodations.Any()
			//	&& !this.Tours.Any())
			//{
			//	FillData();
			//}
			ClearSearchList();
		}

		public void ClearSearchList()
		{
			SearchList = this.Tours.Include(t => t.Company).Include(t => t.City).ToList();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Claim>().HasKey(u => new { u.TourId, u.UserId});
		}

		private void AddRoles()
		{
			var adminRole = new Role { Name = "admin" };
			var companyRole = new Role { Name = "company" };
			var userRole = new Role { Name = "user" };
			Roles.AddRange(adminRole, companyRole, userRole);
			this.SaveChanges();
		}

		private void AddDefaultCountries()
		{
			var country1 = new Country { Name = "Россия" };
			var country2 = new Country { Name = "Турция" };
			
			Countries.AddRange(country1, country2);
			this.SaveChanges();
		}

		private void AddAdmin()
		{
			var admin = new User
			{
				Email = "admin",
				Password = "stmr113",
				Role = Roles.First(r => r.Name == "admin"),
			};
			Users.AddRange(admin);
			this.SaveChanges();
		}

		private void AddDefaultCities()
		{
			var defaultCountry = Countries.First(c => c.Name == "Россия");
			var city1 = new City { Name = "Москва", Country = defaultCountry };
			var city2 = new City { Name = "Екатеринбург", Country = defaultCountry };
			var city3 = new City { Name = "Санкт-Петербург", Country = defaultCountry };
			var city4 = new City { Name = "Анапа", Country = defaultCountry };
			var city5 = new City { Name = "Челябинск", Country = defaultCountry };
			Cities.AddRange(city1, city2, city3, city4, city5);
			this.SaveChanges();
		}
	}
}
