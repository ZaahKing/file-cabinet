using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace FileCabinetGenerator
{
    static class Program
    {
        static void Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-t", "output-type" },
                { "--output-type", "output-type" },
                { "-o", "output" },
                { "--output", "output" },
                { "-a", "records-amount" },
                { "--records-amount", "records-amount" },
                { "-i", "start-id" },
                { "--start-id", "start-id" },
            };
            Console.WriteLine("Hello World!");
            var appConfig = new ConfigurationBuilder()
                .AddCommandLine(args, switchMappings)
                .Build();

            string fileType = appConfig["output-type"], fileName = appConfig["output"];

            if (string.IsNullOrEmpty(fileType)
                || string.IsNullOrEmpty(fileName)
                || string.IsNullOrEmpty(appConfig["records-amount"])
                || string.IsNullOrEmpty(appConfig["start-id"])
                || !int.TryParse(appConfig["records-amount"], out var recordAmount)
                || !int.TryParse(appConfig["start-id"], out var startId))
            {
                Console.WriteLine("Wrong param.");
                Console.WriteLine("Full input fom exemple:");
                Console.WriteLine(@"FileCabinetGenerator.exe --output-type=csv --output=d:\data\records.csv --records-amount=10000 --start-id=30");
                Console.WriteLine("Short pattern:");
                Console.WriteLine(@"FileCabinetGenerator.exe -t xml -o c:\users\myuser\records.xml -a 5000 -i 45");
                return;
            }

            Console.WriteLine($"output-type = {fileType}");
            Console.WriteLine($"output = {fileName}");
            Console.WriteLine($"records-amount = {recordAmount}");
            Console.WriteLine($"start-id = {startId}");
        }
    }
}
