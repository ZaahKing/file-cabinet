using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Remove record command.
    /// </summary>
    internal class RemoveCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("remove", StringComparison.CurrentCultureIgnoreCase))
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

                Program.FileCabinetService.RemoveRecord(id);
                Console.WriteLine($"Record #{id} is removed.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
