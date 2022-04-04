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
			//if (!this.Users.Any()) AddAdmin();
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

		private void FillData()
		{
			if (!this.Countries.Any()) AddDefaultCountries();
			var country1 = Countries.First(c => c.Name == "Россия");
			var country2 = Countries.First(c => c.Name == "Турция");

			var city1 = new City
			{
				Name = "Москва",
				Country = country1,
			};
			var city2 = new City
			{
				Name = "Екатеринбург",
				Country = country1,
			};
			var city3 = new City
			{
				Name = "Челябинск",
				Country = country1,
			};
			var city4 = new City
			{
				Name = "Сочи",
				Country = country1,
			};
			var city5 = new City
			{
				Name = "Анапа",
				Country = country1,
			};
			Cities.AddRange(city1, city2, city3, city4, city5);

			var company1 = new Company
			{
				Email = "Coral@gmail.com",
				Password = "0101",
				PhoneNumber = "504949439",
				Name = "CoralTravel",
				Description = "Известный туроператор Известный туроператор Известный туроператор Известный туроператор" +
				"Известный туроператор Известный туроператор Известный туроператорИзвестный туроператор Известный туроператор" +
				"Известный туроператор Известный туроператорИзвестный туроператорИзвестный туроператор Известный туроператор Известный туроператор" +
				"Известный туроператорИзвестный туроператорИзвестный туроператорИзвестный туроператорИзвестный туроператорИзвестный туроператор" +
				" Известный туроператорИзвестный туроператорИзвестный туроператор Известный туроператор Известный туроператорИзвестный туроператор" +
				"Известный туроператор Известный туроператор Известный туроператор ",
			};
			var company2 = new Company
			{
				Email = "UralTour@mail.ru",
				Password = "1000100010001",
				PhoneNumber = "89514858848",
				Name = "UralTours",
				Description = "Уральское туристическое агенство Уральское туристическое агенство Уральское " +
				"туристическое агенство Уральское туристическое агенство " +
				"Уральское туристическое агенство Уральское туристическое агенство Уральское туристическое агенство " +
				"Уральское туристическое агенство Уральское туристическое агенство" +
				" Уральское туристическое агенство Уральское туристическое агенство Уральское туристическое агенство " +
				"Уральское туристическое агенство Уральское туристическое агенство",
			};
			Companies.AddRange(company1, company2);

			var user1 = new User
			{
				Email = "Pols1@mail.ru",
				Password = "Pols123456789",
				PhoneNumber = "89528477575"
			};
			var user2 = new User
			{
				Email = "Pols2tel@mail.ru",
				Password = "Pols123456789",
				PhoneNumber = "89528377575"
			};
			var user3 = new User
			{
				Email = "testuser@mail.ru",
				Password = "user123456789",
				PhoneNumber = "89528477939"
			};
			var user4 = new User
			{
				Email = "duraselTest@mail.ru",
				Password = "0000",
				PhoneNumber = "88005553535"
			};
			Users.AddRange(user1, user2, user3, user4);
			this.SaveChanges();

			var accommodation1 = new Accommodation
			{
				Name = "Отель ТестовыйОтель",
				Description = "Отель для проверки работы приложения",
				Address = "Тестовая улица 5",
				City = city1,
			};
			var accommodation2 = new Accommodation
			{
				Name = "Московский отель",
				Description = "Отель в москве для проверки приложения",
				Address = "Главня 102",
				City = city1,
			};
			var accommodation3 = new Accommodation
			{
				Name = "Челябинский отель 1",
				Description = "Тестовый отель в челябинске",
				Address = "Улица 10",
				City = city3,
			};
			var accommodation4 = new Accommodation
			{
				Name = "Челябинский отель 2",
				Description = "Тестовый отель в челябинске",
				Address = "Улица 11",
				City = city3,
			};
			var accommodation5 = new Accommodation
			{
				Name = "Челябинский отель 3",
				Description = "Тестовый отель в челябинске",
				Address = "Улица 12",
				City = city3,
			};
			var accommodation6 = new Accommodation
			{
				Name = "Отель у моря",
				Description = "Тестовый отель у моря",
				Address = "Берег :(",
				City = city5,
			};
			var accommodation7 = new Accommodation
			{
				Name = "Центральный",
				Description = "Отель в центре города",
				Address = "Ок 27",
				City = city2,
			};
			Accommodations.AddRange(accommodation1, accommodation2, accommodation3,
			accommodation4, accommodation5, accommodation6, accommodation7);
			this.SaveChanges();

			var tour1 = new Tour
			{
				Title = "Тестовая поездка",
				Description = "Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения",
				MaxTicketNumber = 10,
				Date = DateTime.Parse("13.03.2002"),
				NightsCount = 5,
				Transport = Transport.Bus,
				Return = true,
				Cost = 8000,
				Company = company2,
				City = city3,
				Accommodation = accommodation4
			};
			var tour2 = new Tour
			{
				Title = "Горная прогулка",
				Description = "Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения",
				MaxTicketNumber = 4,
				Date = DateTime.Parse("04.05.2022"),
				NightsCount = 0,
				Transport = Transport.Car,
				Return = true,
				Cost = 1000,
				Company = company2,
				City = city3,
				Accommodation = null
			};
			var tour3 = new Tour
			{
				Title = "Экскурсия по Москве",
				Description = "Пробный тур для проверки приложения (Москва)" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения",
				MaxTicketNumber = 20,
				Date = DateTime.Parse("26.06.2022"),
				NightsCount = 1,
				Transport = Transport.Bus,
				Return = true,
				Cost = 5000,
				Company = company2,
				City = city1,
				Accommodation = accommodation2
			};
			var tour4 = new Tour
			{
				Title = "Отдых на море",
				Description = "Пробный тур для проверки приложения (Перелет)" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения",
				MaxTicketNumber = 50,
				Date = DateTime.Parse("26.06.2022"),
				NightsCount = 7,
				Transport = Transport.Airplane,
				Return = true,
				Cost = 25000,
				Company = company1,
				City = city5,
				Accommodation = accommodation6
			};
			var tour5 = new Tour
			{
				Title = "Экскурсия по Екатеринбургу",
				Description = "Пробный тур для проверки приложения (Поезд)" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения",
				MaxTicketNumber = 30,
				Date = DateTime.Parse("01.04.2022"),
				NightsCount = 1,
				Transport = Transport.Train,
				Return = true,
				Cost = 3000,
				Company = company2,
				City = city2,
				Accommodation = accommodation7
			};

			var tour6 = new Tour
			{
				Title = "Отдых на море",
				Description = "Пробный тур для проверки приложения (Перелет)" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения" +
				"Пробный тур для проверки приложения Пробный тур для проверки приложения Пробный тур для проверки приложения",
				MaxTicketNumber = 50,
				Date = DateTime.Parse("26.06.2022"),
				NightsCount = 7,
				Transport = Transport.Airplane,
				Return = true,
				Cost = 25000,
				Company = company1,
				City = city5,
				Accommodation = accommodation6
			};
			Tours.AddRange(tour1, tour2, tour3, tour4, tour5);
			this.SaveChanges();

			user1.Tours.Add(tour1);
			user1.Tours.Add(tour3);
			user2.Tours.Add(tour4);
			user2.Tours.Add(tour2);
			user3.Tours.Add(tour4);
			user3.Tours.Add(tour3);
			user3.Tours.Add(tour2);
			user3.Tours.Add(tour1);

			this.SaveChanges();
		}

		private void AddAdmin()
		{
			var admin = new User
			{
				Email = "admin",
				Password = "admin",
			};
			Users.AddRange(admin);
			this.SaveChanges();
		}

	}
}
