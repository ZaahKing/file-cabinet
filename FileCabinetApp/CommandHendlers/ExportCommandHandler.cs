using System;
using System.IO;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Export command.
    /// </summary>
    internal class ExportCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public ExportCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                (string format, string fileName) = CommandHandleBase.SplitParam(commandRequest.Parameters);
                if (File.Exists(fileName))
                {
                    Console.Write($"File is exist - rewrite {fileName}? [Y/n] ");
                    char output = char.ToUpper(Console.ReadLine()[0]);
                    if (output != 'Y')
                    {
                        return;
                    }
                }

                FileCabinetServiceSnapshot snapshot = this.service.MakeSnapshot();
                try
                {
                    using (StreamWriter writer = new (fileName))
                    {
                        switch (format)
                        {
                            case "csv":
                                snapshot.SaveToCSV(new FileCabinetRecordCsvWriter(writer));
                                break;

                            case "xml":
                                {
                                    using FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter = new (writer);
                                    snapshot.SaveToXML(fileCabinetRecordXmlWriter);
                                    break;
                                }

                            default:
                                Console.WriteLine("Format is not supported.");
                                break;
                        }
                    }

                    Console.WriteLine($"All records are exported to file {fileName}.");
                }
                catch (IOException)
                {
                    Console.WriteLine($"Export failed: can't open file {fileName}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
