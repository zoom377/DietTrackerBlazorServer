using Microsoft.VisualStudio.TestTools.UnitTesting;
using DietTrackerBlazorServer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietTrackerBlazorServer;
using DietTrackerBlazorServer.Model;

namespace DietTrackerBlazorServer.Tests
{
    [TestClass()]
    public class StatsTests
    {
        [TestMethod()]
        public void GetInterpolatedDataPointValueTest()
        {
            List<HealthDataPoint> exampleData = new List<HealthDataPoint>
            {
                new HealthDataPoint{Date = new DateTime(1000), Value = 2},
                new HealthDataPoint{Date = new DateTime(2000), Value = 7},
                new HealthDataPoint{Date = new DateTime(3000), Value = 5},
                new HealthDataPoint{Date = new DateTime(4000), Value = 9},
                new HealthDataPoint{Date = new DateTime(5000), Value = 4}
            };

            var value = Utilities.GetInterpolatedDataPointValue(exampleData, new DateTime(750));
            Assert.IsTrue(Utilities.AreEqual(value, 2));

            value = Utilities.GetInterpolatedDataPointValue(exampleData, new DateTime(6500));
            Assert.IsTrue(Utilities.AreEqual(value, 4));

            value = Utilities.GetInterpolatedDataPointValue(exampleData, new DateTime(3500));
            Assert.IsTrue(value > 5);

            value = Utilities.GetInterpolatedDataPointValue(exampleData, new DateTime(3500));
            Assert.IsTrue(value < 9);

        }
    }
}