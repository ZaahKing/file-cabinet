using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Edit command.
    /// </summary>
    internal class EditCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("edit", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!int.TryParse(commandRequest.Parameters, out var id))
                {
                    Console.WriteLine("Need a numeric parameter.");
                    return;
                }

                if (Program.FileCabinetService.FindRecordById(id) is null)
                {
                    Console.WriteLine($"Record #{id} is not exist.");
                    return;
                }

                var record = Program.GetFileCabinetRecordFromOutput();
                record.Id = id;
                Program.FileCabinetService.EditRecord(record);
                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
