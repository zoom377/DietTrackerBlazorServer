using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Services
{

    public interface ICorrelationCalculator
    {
        public Task<Dictionary<FoodType, Dictionary<HealthMetric, double>>> GetAllCorrelations();


    }

    public class PearsonsRCorrelation : ICorrelationCalculator
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly AuthenticationStateProvider _authProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public PearsonsRCorrelation(IDbContextFactory<ApplicationDbContext> dbContext,
            AuthenticationStateProvider authProvider,
            UserManager<ApplicationUser> userManager)
        {
            this._dbContextFactory = dbContext;
            this._authProvider = authProvider;
            this._userManager = userManager;
        }


        /// <summary>
        /// Calculate the Pearson's P correlation value between a FoodType and HealthMetric.
        /// </summary>
        /// <param name="foodType"></param>
        /// <param name="healthMetric"></param>
        /// <returns>Pearson's P: A value from -1 to 1 indicating the strength and direction of a correlation between two variables.</returns>
        async Task<double> CalculateCorrelation(FoodType foodType, HealthMetric healthMetric)
        {

            var userId = _userManager.GetUserId((await _authProvider.GetAuthenticationStateAsync()).User);
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {

                var foodDataPoints = await dbContext.FoodDataPoints
                    .Where(e => e.ApplicationUserId == userId)
                    .Where(e => e.FoodType == foodType)
                    .AsNoTracking()
                    .ToListAsync();

                var healthDataPoints = await dbContext.HealthDataPoints
                    .Where(e => e.ApplicationUserId == userId)
                    .Where(e => e.HealthMetric == healthMetric)
                    .AsNoTracking()
                    .ToListAsync();

                List<double> foodEvents = new List<double>();
                List<double> healthMetricValues = new List<double>();

                foreach (var healthDataPoint in healthDataPoints)
                {
                    double foodEvent = 0.0; //Represents whether the food type was consumed recently. 0 = no; 1 = yes
                    double healthMetricValue = healthDataPoint.Value;

                    //Add 1 to foodevent for each time within timespan before this point a food was eaten
                    foodEvent += foodDataPoints.Where(e => healthDataPoint.Date >= e.Date
                            && healthDataPoint.Date < e.Date + new TimeSpan(1, 0, 0, 0)).Count();

                    //If a food was eaten within a timespan before this data point, 
                    //if (foodDataPoints.Any(e => healthDataPoint.Date >= e.Date
                    //    && healthDataPoint.Date < e.Date + new TimeSpan(1, 0, 0, 0)))
                    //{
                    //    foodEvent = 1.0;
                    //}

                    foodEvents.Add(foodEvent);
                    healthMetricValues.Add(healthMetricValue);
                    
                }

                return Correlation.Pearson(foodEvents, healthMetricValues);

            }
        }

        public async Task<Dictionary<FoodType, Dictionary<HealthMetric, double>>> GetAllCorrelations()
        {
            Dictionary<FoodType, Dictionary<HealthMetric, double>> correlations = new Dictionary<FoodType, Dictionary<HealthMetric, double>>();

            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                var userId = _userManager.GetUserId((await _authProvider.GetAuthenticationStateAsync()).User);

                var foodTypes = await dbContext.FoodTypes
                    .Where(e => e.ApplicationUserId == userId)
                    .AsNoTracking()
                    .ToListAsync();

                var healthMetrics = await dbContext.HealthMetrics
                    .Where(e => e.ApplicationUserId == userId)
                    .AsNoTracking()
                    .ToListAsync();

                //Correlation algorithm:
                //Using Pearson's P, which takes a set of data for each of 2 variables and provides a value between
                //-1 and 1, indicating the direction and strength of the correlation.

                foreach (var foodType in foodTypes)
                {
                    correlations.Add(foodType, new Dictionary<HealthMetric, double>());
                    foreach (var metric in healthMetrics)
                    {

                        double correlation = await CalculateCorrelation(foodType, metric);

                        correlations[foodType].Add(metric, correlation);
                    }
                }
            }

            return correlations;
        }
    }
}
