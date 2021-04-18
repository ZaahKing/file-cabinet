using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// List command.
    /// </summary>
    internal class ListCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <param name="printer">Printer.</param>
        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "list";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            this.printer(this.Service.GetRecords());
        }
    }
}
