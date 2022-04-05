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

			AddDefaultCountries();
			if (!this.Users.Any()) AddAdmin();
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

		private void AddDefaultCountries()
		{
			if (!this.Countries.Any())
			{
				var country1 = new Country { Name = "Россия" };
				var country2 = new Country { Name = "Турция" };
				Countries.AddRange(country1, country2);
			}
		}

		private void AddAdmin()
		{
			var admin = new User
			{
				Email = "admin",
				Password = "stmr113",
			};
			Users.AddRange(admin);
			this.SaveChanges();
		}
	}
}
