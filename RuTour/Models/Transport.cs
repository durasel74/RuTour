namespace RuTour.Models
{
	public enum Transport
	{
		None,
		Bus,
		Airplane,
		Train,
		Ship,
		Car,
	}

	static class DurationExtensions
	{
		public static string ToStringRu(this Transport transport)
		{
			switch (transport)
			{
				case Transport.None: return "Нет";
				case Transport.Bus: return "Автобус";
				case Transport.Airplane: return "Самолет";
				case Transport.Train: return "Поезд";
				case Transport.Ship: return "Корабль";
				case Transport.Car: return "Машина";
				default: return "Нет";
			}
		}
	}
}
