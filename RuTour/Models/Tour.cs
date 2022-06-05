using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace RuTour.Models
{
	public class Tour
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int MaxTicketNumber { get; set; } 
		public int NightsCount { get; set; }
		public DateTime Date { get; set; }
		public Transport Transport { get; set; } = Transport.None;
		public bool Return { get; set; }
		public decimal Cost { get; set; }

		public int CompanyId { get; set; }
		public Company Company { get; set; }

		public int CityId { get; set; }
		public City City { get; set; }

		public int? AccommodationId { get; set; }
		public Accommodation Accommodation { get; set; }

		public List<Claim> Claimes { get; set; } = new List<Claim>();

		[NotMapped]
		public List<Claim> NoAcceptedClaimes { get { return Claimes.Where(c => c.Accepted == false).ToList(); } }

		[NotMapped]
		public List<Claim> AcceptedClaimes { get { return Claimes.Where(c => c.Accepted == true).ToList(); } }

		public string DateString { get { return Date.ToString("yyyy-MM-dd"); } }
		public string TransportString { get { return Transport.ToStringRu(); } }
		public string ReturnString { get { return Return ? "Есть" : "Нет"; } }
		public string CostString { get { return Cost.ToString(CultureInfo.CreateSpecificCulture("en-GB")); } }
		public int TicketsLeft
		{
			get
			{
				int bookedClimes = 0;
				foreach (var claim in AcceptedClaimes)
					bookedClimes += claim.Count;
				return MaxTicketNumber - bookedClimes;
			}
		}
	}
}
