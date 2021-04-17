using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Stat command.
    /// </summary>
    internal class StatCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("stat", StringComparison.CurrentCultureIgnoreCase))
            {
                var recordsCount = Program.FileCabinetService.GetStat();
                Console.WriteLine($"{recordsCount} record(s). Including {Program.FileCabinetService.GetStatDeleted()} is ready to purging.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
