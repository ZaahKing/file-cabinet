using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Create record comand.
    /// </summary>
    internal class CreateCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("create", StringComparison.CurrentCultureIgnoreCase))
            {
                int id = Program.FileCabinetService.CreateRecord(Program.GetFileCabinetRecordFromOutput());
                Console.WriteLine($"Record #{id} is created.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
