using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// List command.
    /// </summary>
    internal class ListCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("list", StringComparison.CurrentCultureIgnoreCase))
            {
                Program.PrintFileCabinetRecordsList(Program.FileCabinetService.GetRecords());
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
