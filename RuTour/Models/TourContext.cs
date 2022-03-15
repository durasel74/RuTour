using Microsoft.EntityFrameworkCore;

namespace RuTour.Models
{
	public class TourContext : DbContext
	{
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public TourContext(DbContextOptions<TourContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
	}
}
