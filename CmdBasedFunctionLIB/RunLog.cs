using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;


namespace CmdBasedFunctionLIB
{
    public class RunLog : TableEntity
    {
        // PK : 99999999 - 20190226
        // RK : Ticks desc
        public DateTime StartDT { get; set; }
        public DateTime? EndDT { get; set; }
        public string Function { get; set; }
        public string Cmd { get; set; }
        public double? Elapsed { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public string Parameters { get; set; }
        public string IP { get; set; }
        public string User { get; set; }

        public RunLog()
        {
            var now = DateTime.UtcNow;
            PartitionKey = (99999999 - (now.Year * 100 * 100 + now.Month * 100 + now.Day)).ToString();
            RowKey = String.Format("{0:D19}", DateTime.MaxValue.Ticks - now.Ticks) + "_" + Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
