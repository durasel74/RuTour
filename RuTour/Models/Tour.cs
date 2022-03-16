using System;
using System.Collections.Generic;

namespace RuTour.Models
{
	public class Tour
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int MaxPeopleNumber { get; set; } 
		public DateTime Date { get; set; }
		public int NightsCount { get; set; }
		public Transport Transport { get; set; } = Transport.None;
		public bool Return { get; set; }
		public decimal Cost { get; set; }

		public int CompanyId { get; set; }
		public Company Company { get; set; }

		public int AccommodationId { get; set; }
		public Accommodation Accommodation { get; set; }

		public List<User> Users { get; set; } = new List<User>();
	}
}
