using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CmdBasedFunctionLIB
{
    public class NOPHandler : ICmdHandler
    {
        public string CmdName { get { return "NOP"; } }

        public async Task<List<Cmd>> Execute(Cmd inputCmd, ILogger log)
        {
            // do nothing
            return await Task.FromResult(new List<Cmd>());
        }
    }
}
