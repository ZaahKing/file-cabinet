using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// List command.
    /// </summary>
    internal class ListCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public ListCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "list";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            Program.PrintFileCabinetRecordsList(this.Service.GetRecords());
        }
    }
}
