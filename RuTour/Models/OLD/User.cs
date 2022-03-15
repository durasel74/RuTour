using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLearn.Model
{
	public class User
	{
		[Key]
		public string login { get; set; }
		public string password { get; set; }
		public DateTime birthDate { get; set; }

		[Column("e-mail")]
		public string email { get; set; }
	}
}
