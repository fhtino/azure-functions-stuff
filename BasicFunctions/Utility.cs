using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunctions
{
    public class Utility
    {
        public static void FakeLongRunning(int seconds)
        {
            var startDT = DateTime.UtcNow;
            while (DateTime.UtcNow.Subtract(startDT).TotalSeconds < seconds)
            {
                var dummy = Math.Sin(DateTime.UtcNow.Ticks);
            }
        }
    }
}
