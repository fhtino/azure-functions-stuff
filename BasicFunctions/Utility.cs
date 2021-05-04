//using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicFunctions
{
    public class Utility
    {
        public static async Task FakeLongRunning(int seconds, int loadPercentage)
        {
            var startDT = DateTime.UtcNow;
            while (DateTime.UtcNow.Subtract(startDT).TotalSeconds < seconds)
            {
                var dummy = Math.Sin(DateTime.UtcNow.Ticks);
                if (DateTime.UtcNow.Millisecond % 100 > loadPercentage)
                {
                    await Task.Delay(100 - loadPercentage);
                }                
            }           
        }
    }
}
