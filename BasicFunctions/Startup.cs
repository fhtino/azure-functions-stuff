using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(BasicFunctions.Startup))]    // <<<===


namespace BasicFunctions
{
    public class Startup : FunctionsStartup
    {
        // Sample from : https://github.com/Azure/azure-functions-dotnet-extensions/blob/master/src/samples/DependencyInjection/Scopes/SampleStartup.cs

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // my startup code...
        }
    }
}
