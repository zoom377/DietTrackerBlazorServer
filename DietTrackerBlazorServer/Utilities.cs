using Blazorise.Charts;
using DietTrackerBlazorServer.Model;

namespace DietTrackerBlazorServer
{
    public static class Utilities
    {
        /// <summary>
        /// Calculates and returns an interpolated value. Example use: display on a Blazorise line chart.
        /// </summary>
        /// <param name="dataPoints">A list of dataPoints assumed to be of the same healthMetric type, sorted by date ascending.</param>
        /// <param name="time">The time for which an interpolated dataPoint value will be returned.</param>
        /// <returns></returns>
        public static double GetInterpolatedDataPointValue(List<HealthDataPoint> dataPoints, DateTime time)
        {
            if (dataPoints.Count == 0)
            {
                return 0;
            }

            for (int i = 0; i < dataPoints.Count; i++)
            {
                if (time < dataPoints[i].Date)
                {
                    if (i == 0)
                    {
                        return dataPoints[i].Value;
                    }
                    else
                    {
                        long prevToTimeDelta = time.Ticks - dataPoints[i - 1].Date.Ticks;
                        long prevToNextDelta = dataPoints[i].Date.Ticks - dataPoints[i - 1].Date.Ticks;
                        double interpRatio = (double)prevToTimeDelta / (double)prevToNextDelta;
                        return dataPoints[i - 1].Value + interpRatio * (dataPoints[i].Value - dataPoints[i - 1].Value);
                    }
                }
            }

            //time is bigger than anything in list. Return value of last data point.
            return dataPoints[dataPoints.Count - 1].Value;
        }

        public static bool AreEqual(double d1, double d2, double tolerance = double.Epsilon)
        {
            if (d1 < d2 - tolerance || d1 > d2 + tolerance)
                return false;

            return true;
        }

        static readonly Dictionary<char, int> _hexValues = new Dictionary<char, int>
        {
            {'0',0 },
            {'1',1 },
            {'2',2 },
            {'3',3 },
            {'4',4 },
            {'5',5 },
            {'6',6 },
            {'7',7 },
            {'8',8 },
            {'9',9 },
            {'a',10 },
            {'b',11 },
            {'c',12 },
            {'d',13 },
            {'e',14 },
            {'f',15 },
        };

        public static int HexCharToByte(char hex)
        {
            return _hexValues[char.ToLower(hex)];
        }

        //public static int HexStringToInt(string hex)
        //{
            
        //}

        public static ChartColor ChartColorFromHex(string color)
        {
            //#FFFFFFFF
            int R = 0;
            int G = 0;
            int B = 0;
            int A = 0;

            if (color.Length >= 9)
            {
                //Includes alpha
                //A is index 7 & 8
                R = HexCharToByte(color[7]) * 16 + HexCharToByte(color[8]);

            }
            if (color.Length >= 7)
            {
                //Excludes alpha
                //R is index 1 & 2
                R = HexCharToByte(color[1]) * 16 + HexCharToByte(color[2]);
                //G is index 3 & 4
                G = HexCharToByte(color[3]) * 16 + HexCharToByte(color[4]);
                //B is index 5 & 6
                B = HexCharToByte(color[5]) * 16 + HexCharToByte(color[6]);
            }

            return ChartColor.FromRgba(((byte)R), (byte)G, (byte)B, (float)A / (float)255);
        }
    }
}
