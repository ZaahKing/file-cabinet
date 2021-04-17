using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Purge command.
    /// </summary>
    internal class PurgeCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("purge", StringComparison.CurrentCultureIgnoreCase))
            {
                int recordCount = Program.FileCabinetService.GetStat();
                Console.WriteLine($"Data storage processing is completed: {Program.FileCabinetService.PurgeStorage()} of {recordCount} records were purged.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
