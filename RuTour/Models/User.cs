using System;
using System.Collections.Generic;

namespace RuTour.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }

		public List<Tour> Tours { get; set; } = new List<Tour>();
	}
}
