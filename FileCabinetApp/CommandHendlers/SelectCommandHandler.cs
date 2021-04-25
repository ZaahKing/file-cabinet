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
        private readonly FileCabinetRecordCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <param name="cache">File cabinet service cache.</param>
        public SelectCommandHandler(IFileCabinetService service, FileCabinetRecordCache cache)
            : base(service)
        {
            this.cache = cache;
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
                list = this.IsCached(commandRequest.Parameters, this.Service.GetRecords());
                selectSection = commandRequest.Parameters;
            }
            else
            {
                string whereSection = commandRequest.Parameters.Substring(whereIndex + whereString.Length + 1);
                selectSection = commandRequest.Parameters.Substring(0, whereIndex);
                var filter = Parser.Parser.Parse(whereSection);
                list = this.IsCached(commandRequest.Parameters, this.Service.GetRecords().Where(x => filter.Execute(x)));
            }

            IEnumerable<string> fields = string.IsNullOrWhiteSpace(selectSection)
                ? SelectorBuilder.GetFieldsNames()
                : selectSection.Split(',').Select(x => x.Trim(' '));

            Printer printer = new (fields);
            printer.Print(list);
        }

        private IEnumerable<FileCabinetRecord> IsCached(string key, IEnumerable<FileCabinetRecord> list)
        {
            if (!this.cache.IsCached(key))
            {
                this.cache.PutCache(key, list.ToList());
            }

            return this.cache.GetCashe(key);
        }
    }
}
