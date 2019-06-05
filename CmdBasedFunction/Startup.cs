using CmdBasedFunctionLIB;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(CmdBasedFunction.Startup))]    // <<<===


namespace CmdBasedFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            Console.WriteLine("STARTUP-CLASS");

            RunLogger.Setup(Utility.GetEnvVar("AzureWebJobsStorage"), Utility.GetEnvVar("RunLoggerTableName"));
            var runLogger = new RunLogger("Startup", "");

            CmdProcessor.RegisterCmdHandler(new NOPHandler());
            CmdProcessor.RegisterCmdHandler(new MULTIHandler());
            CmdProcessor.RegisterCmdHandler(new COLLECTHandler());

            runLogger.End(-1, "");
        }
    }
}
