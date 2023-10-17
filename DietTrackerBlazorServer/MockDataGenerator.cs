using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;

namespace DietTrackerBlazorServer
{
    public static class MockDataGenerator
    {

        public static List<Object> GenerateData(string userId, TimeSpan backwardsSpan)
        {
            var data = new List<Object>();

            //Generate data
            //Foodtypes
            var bacon = new FoodType { Name = "Bacon", ApplicationUserId = userId };
            data.Add(bacon);
            var lettuce = new FoodType { Name = "Lettuce", ApplicationUserId = userId };
            data.Add(lettuce);
            var cheese = new FoodType { Name = "Cheese", ApplicationUserId = userId };
            data.Add(cheese);

            var anxiety = new HealthMetric { Name = "Anxiety", ApplicationUserId = userId, Color = "#00ff00" };
            data.Add(anxiety);
            var energy = new HealthMetric { Name = "Energy", ApplicationUserId = userId, Color = "#00ffff" };
            data.Add(energy);
            var sleep = new HealthMetric { Name = "Sleep", ApplicationUserId = userId, Color = "#0000ff" };
            data.Add(sleep);

            int count = 1_000;
            DateTime startTime = DateTime.Now.Subtract(backwardsSpan);
            TimeSpan interval = backwardsSpan / count;

            //Anxiety
            //Positive correlation
            for (int i = 0; i < count; i++)
            {
                var rand = Random.Shared.Next(0, 10);
                var date = startTime + interval * Random.Shared.Next(0, count);
                data.Add(new HealthDataPoint
                {
                    ApplicationUserId = userId,
                    HealthMetric = anxiety,
                    Value = rand,
                    Date = date
                });

                data.Add(new FoodDataPoint
                {
                    ApplicationUserId = userId,
                    FoodType = bacon,
                    Date = date
                });
            }

            //Energy
            //Random correlation
            for (int i = 0; i < count; i++)
            {
                var rand = Random.Shared.Next(0, 10);
                var metricDate = startTime + interval * Random.Shared.Next(0, count);
                var foodDate = startTime + interval * Random.Shared.Next(0, count);
                data.Add(new HealthDataPoint
                {
                    ApplicationUserId = userId,
                    HealthMetric = energy,
                    Value = rand,
                    Date = metricDate
                });

                data.Add(new FoodDataPoint
                {
                    ApplicationUserId = userId,
                    FoodType = lettuce,
                    Date = foodDate
                });
            }

            //Sleep
            //Negative correlation
            //for (int i = 0; i < count; i++)
            //{
            //    var rand = Random.Shared.Next(0, 10);
            //    var date = startTime + interval * Random.Shared.Next(0, count);
            //    data.Add(new HealthDataPoint
            //    {
            //        ApplicationUserId = userId,
            //        HealthMetric = anxiety,
            //        Value = rand,
            //        Date = date
            //    });

            //    data.Add(new FoodDataPoint
            //    {
            //        ApplicationUserId = userId,
            //        FoodType = bacon,
            //        Date = date
            //    });
            //}

            //Health data points
            //Food data points

            return data;
        }

    }
}
