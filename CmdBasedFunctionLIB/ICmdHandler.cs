using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace CmdBasedFunctionLIB
{
    public interface ICmdHandler
    {
        string CmdName { get; }

        Task<List<Cmd>> Execute(Cmd inputCmd, ILogger log);
    }
}

