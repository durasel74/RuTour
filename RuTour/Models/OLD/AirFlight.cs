using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearn.Model
{
	public class AirFlight
	{
		[Key]
		public long flightNumber { get; set; }
		public string airline { get; set; }
		public DateTime departureDate { get; set; }
		public DateTime arrivalDate { get; set; }
		public string route { get; set; }
	}
}
