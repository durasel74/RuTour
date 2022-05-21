namespace RuTour.Models
{
	public class Claim
	{
		public Tour Tour { get; set; }
		public int TourId { get; set; }

		public User User { get; set; }
		public int UserId { get; set; }

		public int Count { get; set; }
		public bool Accepted { get; set; }

		public decimal TotalCost
		{
			get
			{
				if (Tour != null) return Tour.Cost * Count;
				else return 0;
			}
		}
	}
}
