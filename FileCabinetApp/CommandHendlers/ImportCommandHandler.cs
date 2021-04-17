using System;
using System.IO;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Import command.
    /// </summary>
    internal class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "import";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            (string format, string fileName) = CommandHandleBase.SplitParam(commandRequest.Parameters);
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
                    count = this.Service.Restore(snapshot);
                    break;
                case "xml":
                    snapshot.LoadFromXml(fileStream);
                    count = this.Service.Restore(snapshot);
                    break;
                default:
                    Console.WriteLine("Format is not supported.");
                    break;
            }

            Console.WriteLine($"{count} records were imported from {fileName}.");
        }
    }
}
