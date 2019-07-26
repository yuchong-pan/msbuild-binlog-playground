using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Logging.StructuredLogger;

namespace MSBuildBinLogPlayground
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintErrorMessage("Argument missing");
            }

            foreach (var binLogFilePath in args)
            {
                var binLogReader = new BinLogReader();

                IEnumerable<Record> records;
                try
                {
                    records = binLogReader.ReadRecords(binLogFilePath);
                }
                catch (Exception e)
                {
                    PrintErrorMessage($"Exception while reading binlog: {e.Message}");
                    return;
                }

                // print the first 10 <Message> events

                var count = 0;

                foreach (var record in records)
                {
                    if (record.Args is BuildMessageEventArgs buildMessageEventArgs)
                    {
                        Console.WriteLine(buildMessageEventArgs.Message);

                        if (++count == 10) {
                            break;
                        }
                    }
                }
            }
        }

        private static void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
