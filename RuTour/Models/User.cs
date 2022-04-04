using System;
using System.Collections.Generic;
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

		public List<Tour> Tours { get; set; } = new List<Tour>();
	}
}
