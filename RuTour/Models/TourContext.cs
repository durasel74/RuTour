using Microsoft.EntityFrameworkCore;

namespace RuTour.Models
{
	public class TourContext : DbContext
	{
		public DbSet<Country> Countries { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Accommodation> Accommodations { get; set; }
		public DbSet<Tour> Tours { get; set; }

		public TourContext(DbContextOptions<TourContext> options)
			: base(options)
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();

			/////////////////////////////////////////////////////////////
			var country1 = new Country { Name = "Россия" };
			var country2 = new Country { Name = "Турция" };
			Countries.AddRange(country1, country2);

			Cities.AddRange(
				new City
				{
					Name = "Москва",
					Country = country1,
					CountryId = country1.Id,
				},
				new City
				{
					Name = "Екатиринбург",
					Country = country1,
					CountryId = country1.Id,
				},
				new City
				{
					Name = "Челябинск",
					Country = country1,
					CountryId = country1.Id,
				},
				new City
				{
					Name = "Сочи",
					Country = country1,
					CountryId = country1.Id,
				},
				new City
				{
					Name = "Анапа",
					Country = country1,
					CountryId = country1.Id,
				}
			);

			var company1 = new Company
			{
				Login = "Coral",
				Email = "Coral@gmail.com",
				Password = "0101",
				PhoneNumber = "504949439",
				Name = "CoralTravel",
				Description = "Известный туроператор",
			};
			var company2 = new Company
			{
				Login = "Ural",
				Email = "UralTour@mail.ru",
				Password = "1000100010001",
				PhoneNumber = "89514858848",
				Name = "UralTours",
				Description = "Уральское туристическое агенство",
			};
			Companies.AddRange(
				company1,
				company2
			);

			var user1 = new User
			{
				Login = "Pols1",
				Email = "Pols1@mail.ru",
				Password = "Pols123456789",
				PhoneNumber = "89528477575"
			};

			var user2 = new User
			{
				Login = "Pols2tel",
				Email = "Pols2tel@mail.ru",
				Password = "Pols123456789",
				PhoneNumber = "89528377575"
			};

			var user3 = new User
			{
				Login = "user",
				Email = "testuser@mail.ru",
				Password = "user123456789",
				PhoneNumber = "89528477939"
			};


			this.SaveChanges();
		}
	}
}
