using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Insert record command.
    /// </summary>
    internal class SelectCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public SelectCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "select";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            string whereString = "where";
            int whereIndex = commandRequest.Parameters.IndexOf(whereString, StringComparison.CurrentCultureIgnoreCase);
            IEnumerable<FileCabinetRecord> list;
            if (whereIndex < 0)
            {
                list = this.Service.GetRecords();
            }
            else
            {
                string whereSection = commandRequest.Parameters.Substring(whereIndex + whereString.Length + 1);
                var filter = Parser.Parser.Parse(whereSection);
                list = this.Service.GetRecords().Where(x => filter.Execute(x));
            }

            int counter = 0;
            foreach (var record in list)
            {
                counter++;
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.DigitKey}, {record.Account}, {record.Sex}");
            }

            if (counter == 0)
            {
                Console.WriteLine("Nothing to display.");
            }
        }
    }
}
