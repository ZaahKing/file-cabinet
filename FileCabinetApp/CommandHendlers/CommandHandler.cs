using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Default command.
    /// </summary>
    public class CommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            this.NextHandler?.Handle(commandRequest);
        }
    }
}
