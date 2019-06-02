using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CmdBasedFunctionLIB
{
    public class COLLECTHandler : ICmdHandler
    {
        public string CmdName { get { return "COLLECT"; } }


        public async Task<List<Cmd>> Execute(Cmd inputCmd, ILogger log)
        {
            string filename = inputCmd.Parameters["filename"];
            log.LogInformation($"Start collecting file {filename}");
            await Task.Delay(1000);
            log.LogInformation($"End collecting file {filename}");
            return null;
        }

    }
}
