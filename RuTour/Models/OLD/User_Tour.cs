using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearn.Model
{
	public class User_Tour
	{
		[Key]
		public string userLogin { get; set; }
		[Key]
		public string tourName { get; set; }
		[Key]
		public DateTime tourDate { get; set; }
	}
}
