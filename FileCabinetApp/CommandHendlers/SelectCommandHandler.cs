using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Helpers;

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
            string selectSection;
            IEnumerable<FileCabinetRecord> list;
            if (whereIndex < 0)
            {
                list = this.Service.GetRecords();
                selectSection = commandRequest.Parameters;
            }
            else
            {
                string whereSection = commandRequest.Parameters.Substring(whereIndex + whereString.Length + 1);
                selectSection = commandRequest.Parameters.Substring(0, whereIndex);
                var filter = Parser.Parser.Parse(whereSection);
                list = this.Service.GetRecords().Where(x => filter.Execute(x));
            }

            IEnumerable<string> fields = string.IsNullOrWhiteSpace(selectSection)
                ? SelectorBuilder.GetFieldsNames()
                : selectSection.Split(',').Select(x => x.Trim(' '));

            Printer printer = new (fields);
            printer.Print(list);
        }
    }
}
