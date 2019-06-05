using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace CmdBasedFunctionLIB
{

    public class RunLogger
    {

        private static string _storageConnString;
        private static string _tableName;
        private static string _tableSasURI;


        public static void Setup(string tableSasURI)
        {
            _tableSasURI = tableSasURI;
        }


        public static void Setup(string storageConnString, string tableName)
        {
            _storageConnString = storageConnString;
            _tableName = tableName;

            CreateTableIfNotExist();
        }


        public static void CreateTableIfNotExist()
        {
            var table = GetCloudTableReference();
            if (!table.Exists())
                table.Create();
        }


        // ----------------------------------------------------------------------

        private RunLog _logEntry;


        public RunLogger(string functionName, string cmdName, string parameters = null, string IP = null, string userName = null)
        {
            _logEntry = new RunLog()
            {
                StartDT = DateTime.UtcNow,
                Function = functionName,
                Cmd = cmdName,
                Parameters = parameters,
                IP = IP,
                User = userName
            };

            InsertOrMerge(_logEntry);
        }


        public void End(int errorCode, string errorDescritpion = null)
        {
            _logEntry.EndDT = DateTime.UtcNow;
            _logEntry.Elapsed = _logEntry.EndDT.Value.Subtract(_logEntry.StartDT).TotalSeconds;
            InsertOrMerge(_logEntry);
        }



        // ----------------------

        private void InsertOrMerge(RunLog logEntry)
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(logEntry);
            var table = GetCloudTableReference();
            var result = table.Execute(insertOrMergeOperation);
        }


        private static CloudTable GetCloudTableReference()
        {
            if (_storageConnString != null && _tableName != null)
            {
                var storageAccount = CloudStorageAccount.Parse(_storageConnString);
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference(_tableName);
                return table;
            }
            else if (_tableSasURI != null)
            {
                return new CloudTable(new Uri(_tableSasURI));
            }
            else
            {
                throw new ApplicationException("Unmanaged");
            }
        }

    }
}
