using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearn.Model
{
	public class Tour
	{
		[Key]
		public string name { get; set; }
		[Key]
		public DateTime date { get; set; }
		public string operatorName { get; set; }
		public string hotelName { get; set; }
		public long flightThere { get; set; }
		public long flightBack { get; set; }
	}
}
