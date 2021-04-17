using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Empty command handler.
    /// </summary>
    internal class EmptyCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (string.IsNullOrWhiteSpace(commandRequest.Command))
            {
                Console.WriteLine("Enter command. 'help' for exemple");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
