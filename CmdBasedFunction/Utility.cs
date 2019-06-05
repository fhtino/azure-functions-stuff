using System;
using System.Collections.Generic;
using System.Text;

namespace CmdBasedFunction
{
    internal class Utility
    {
        public static string GetEnvVar(string key)
        {
            string value = Environment.GetEnvironmentVariable(key);
            if (value == null)
                throw new ApplicationException($"Key not found : {key}");
            return value;
        }
    }
}
