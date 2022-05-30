using System.ComponentModel.DataAnnotations.Schema;

namespace RuTour.Models
{
	public class User
	{
		[NotMapped]
		public TourContext DB { get; set; }

		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string? PhoneNumber { get; set; }
		public decimal Cash { get; set; }

		public int RoleId { get; set; }
		public Role Role { get; set; }
		public List<Claim> Claimes { get; set; } = new List<Claim>();
	}
}
