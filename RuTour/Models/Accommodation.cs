namespace RuTour.Models
{
	public class Accommodation
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Address { get; set; }

		public int CityId { get; set; }
		public City City { get; set; }

		public List<Tour> Tours { get; set; }
	}
}
