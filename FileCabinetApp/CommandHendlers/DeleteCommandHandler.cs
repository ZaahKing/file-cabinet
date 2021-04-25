using System;
using System.Linq;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Insert record command.
    /// </summary>
    internal class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "delete";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            string whereString = "where";
            int whereIndex = commandRequest.Parameters.IndexOf(whereString, StringComparison.CurrentCultureIgnoreCase);
            string whereSection = commandRequest.Parameters.Substring(whereIndex + whereString.Length + 1);
            var filter = Parser.Parser.Parse(whereSection);
            var list = this.Service.GetRecords().Where(x => filter.Execute(x)).ToList();

            foreach (var record in list)
            {
                this.Service.RemoveRecord(record.Id);
            }

            if (list.Count == 0)
            {
                Console.WriteLine("No records to delete.");
            }
            else if (list.Count == 1)
            {
                Console.WriteLine($"Record #{list[0].Id} is deleted.");
            }
            else
            {
                Console.WriteLine($"Records {string.Join(", ", list.Select(x => $"#{x.Id}").ToArray())} are deleted. ");
            }
        }
    }
}
