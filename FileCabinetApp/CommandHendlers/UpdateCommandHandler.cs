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

        /// <summary>
        /// On update;
        /// </summary>
        public event EventHandler OnUpdate;

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
            var filter = Parser.Parser.Parse(whereSection);
            var list = this.Service.GetRecords().Where(x => filter.Execute(x)).ToList();
            var editRecord = setSectionPairList.GetRecordEditor();

            int counter = 0;
            foreach (var record in list)
            {
                editRecord?.Invoke(record);
                this.Service.EditRecord(record);
                counter++;
            }

            if (counter == 0)
            {
                Console.WriteLine("No records to edit.");
            }
            else if (counter == 1)
            {
                Console.WriteLine("One record has edited.");
            }
            else
            {
                Console.WriteLine($"{counter} records have edited.");
            }

            this.OnUpdate?.Invoke(this, new EventArgs());
        }
    }
}
