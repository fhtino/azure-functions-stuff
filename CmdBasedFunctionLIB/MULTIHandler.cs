using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CmdBasedFunctionLIB
{
    public class MULTIHandler : ICmdHandler
    {
        public string CmdName { get { return "MULTI"; } }

        public async Task<List<Cmd>> Execute(Cmd inputCmd, ILogger log)
        {
            var outCmdList = new List<Cmd>();

            for (int i = 0; i < int.Parse(inputCmd.Parameters["count"]); i++)
            {
                outCmdList.Add(new Cmd("COLLECT", "filename", $"fakefilename_{i}.txt"));
                await Task.Delay(100);
            }

            return outCmdList;
        }
    }
}
