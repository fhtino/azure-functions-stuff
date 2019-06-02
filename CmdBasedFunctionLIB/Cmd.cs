using System;
using System.Collections.Generic;
using System.Text;

namespace CmdBasedFunctionLIB
{
    public class Cmd
    {
        public string ID { get; set; }
        public DateTime CreationDT { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Parameters { get; set; }


        public Cmd()
        {
            CreationDT = DateTime.UtcNow;
            ID = CreationDT.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString().Replace("-", "");
            Parameters = new Dictionary<string, string>();
        }


        public Cmd(string name, params string[] parameters) : this()
        {
            Name = name;

            for (int i = 0; i < parameters.Length; i += 2)
            {
                Parameters.Add(parameters[i], parameters[i + 1]);
            }
        }

    }
}
