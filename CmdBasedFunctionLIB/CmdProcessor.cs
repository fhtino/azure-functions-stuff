using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmdBasedFunctionLIB
{

    public class CmdProcessor
    {
        private static Dictionary<string, ICmdHandler> _dict = new Dictionary<string, ICmdHandler>();

        public static void RegisterCmdHandler(ICmdHandler handler)
        {
            _dict.Add(handler.CmdName, handler);
        }

        public static async Task<List<Cmd>> Execute(Cmd inputCmd, ILogger log)
        {
            if (!_dict.ContainsKey(inputCmd.Name))
                throw new ApplicationException($"Handler not found for command {inputCmd.Name}");

            return await _dict[inputCmd.Name].Execute(inputCmd, log);
        }
    }

}
