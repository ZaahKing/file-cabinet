using System;
using System.IO;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Import command.
    /// </summary>
    internal class ImportCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("import", StringComparison.CurrentCultureIgnoreCase))
            {
                (string format, string fileName) = Program.SplitParam(commandRequest.Parameters);
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"File \"{fileName}\"is not exist.");
                    return;
                }

                FileStream fileStream;
                try
                {
                    fileStream = File.OpenRead(fileName);
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't open file");
                    return;
                }

                int count = 0;
                var snapshot = new FileCabinetServiceSnapshot();
                switch (format)
                {
                    case "csv":
                        snapshot.LoadFromCSV(fileStream);
                        count = Program.FileCabinetService.Restore(snapshot);
                        break;
                    case "xml":
                        snapshot.LoadFromXml(fileStream);
                        count = Program.FileCabinetService.Restore(snapshot);
                        break;
                    default:
                        Console.WriteLine("Format is not supported.");
                        break;
                }

                Console.WriteLine($"{count} records were imported from {fileName}.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
