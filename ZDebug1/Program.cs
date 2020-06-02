using CmdBasedFunctionLIB;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ZDebug1
{
    class Program
    {
        static void Main(string[] args)
        {

            // ----------------------------------------------------------

            var cmd1 = new Cmd();
            var cmd2 = new Cmd("NOP", "p1", "v1", "p2", "v2");

            string json1 = JsonConvert.SerializeObject(cmd1);
            string json2 = JsonConvert.SerializeObject(cmd2);

            Console.WriteLine(json1);
            Console.WriteLine(json2);

            var cmd1bis = JsonConvert.DeserializeObject<Cmd>(json1);
            var cmd2bis = JsonConvert.DeserializeObject<Cmd>(json2);

            string json1bis = JsonConvert.SerializeObject(cmd1bis);
            string json2bis = JsonConvert.SerializeObject(cmd2bis);

            Console.WriteLine(json1bis);
            Console.WriteLine(json2bis);

            // ----------------------------------------------------------            


            Utility.FakeLongRunning(10, 50).Wait();


        }


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
}
