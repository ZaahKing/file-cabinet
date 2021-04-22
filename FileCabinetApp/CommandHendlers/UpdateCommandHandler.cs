using System;
using System.Linq;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Insert record command.
    /// </summary>
    internal class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "update";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            string setString = "set";
            string whereString = "where";

            int setIndex = commandRequest.Parameters.IndexOf(setString, StringComparison.CurrentCultureIgnoreCase);
            int whereIndex = commandRequest.Parameters.IndexOf(whereString, StringComparison.CurrentCultureIgnoreCase);
            setIndex = setIndex + setString.Length + 1;

            string setSection = commandRequest.Parameters.Substring(setIndex, whereIndex - setIndex);
            var setSectionPairList = setSection.GetSetPairs();

            string whereSection = commandRequest.Parameters.Substring(whereIndex + whereString.Length + 1);
            var whereSectionPairList = whereSection.GetWherePairs();
            var list = this.Service.GetRecords().GetFilteredList(whereSectionPairList).ToList();
            var editRecord = setSectionPairList.GetRecordEditor();

            foreach (var record in list)
            {
                editRecord?.Invoke(record);
                this.Service.EditRecord(record);
            }

            if (list.Count == 0)
            {
                Console.WriteLine("No records to edit.");
            }
            else if (list.Count == 1)
            {
                Console.WriteLine("One record has edited.");
            }
            else
            {
                Console.WriteLine($"{list.Count} records have edited.");
            }
        }
    }
}
