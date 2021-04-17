using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// List command.
    /// </summary>
    internal class ListCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public ListCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("list", StringComparison.CurrentCultureIgnoreCase))
            {
                Program.PrintFileCabinetRecordsList(this.service.GetRecords());
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
