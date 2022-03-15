namespace RuTour.Models
{
    public static class SampleData
    {
        public static void Initialize(TourContext context)
        {
            if (!context.Cities.Any())
            {
                context.Cities.AddRange(
                    new City
                    {
                        Id = 1,
                        Name = "OK"
                    },
                    new City
                    {
                        Id = 2,
                        Name = "OK2"
                    },
                    new City
                    {
                        Id = 3,
                        Name = "OK3"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
