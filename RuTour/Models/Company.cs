namespace RuTour.Models
{
	public class Company
	{
		public Guid Id { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<Tour> Tours { get; set; } = new List<Tour>();
	}
}
