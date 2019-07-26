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
                // PrintFirstTenMessages(binLogFilePath);
                // PrintProjects(binLogFilePath);
                PrintTargets(binLogFilePath);
            }
        }

        private static void PrintFirstTenMessages(string binLogFilePath)
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

        private static void PrintProjects(string binLogFilePath)
        {
            var build = BinaryLog.ReadBuild(binLogFilePath);

            build.VisitAllChildren<Project>(p => Console.WriteLine(p.Name));
        }

        private static void PrintTargets(string binLogFilePath)
        {
            var build = BinaryLog.ReadBuild(binLogFilePath);

            build.VisitAllChildren<Project>(p =>
            {
                p.VisitAllChildren<Target>(t => Console.WriteLine(t.Name));
            });
        }

        private static void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
