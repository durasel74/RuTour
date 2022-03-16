namespace RuTour.Models
{
	public class City
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int CountryId { get; set; }
		public Country Country { get; set; }

		public List<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
	}
}
