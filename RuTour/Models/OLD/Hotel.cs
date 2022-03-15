using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearn.Model
{
	public class Hotel
	{
		[Key]
		public string name { get; set; }
		public string description { get; set; }
		public string cityName { get; set; }
	}
}
