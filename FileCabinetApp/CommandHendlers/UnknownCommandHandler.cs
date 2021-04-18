using System;
using System.Linq;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Unknown.
    /// </summary>
    internal class UnknownCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (HelpData.HelpMessages.Any(x => commandRequest.Command.Equals(x[HelpData.CommandHelpIndex], StringComparison.CurrentCultureIgnoreCase)))
            {
                this.NextHandler?.Handle(commandRequest);
            }
            else
            {
                Console.WriteLine($"There is no '{commandRequest.Command}' command.");
                Console.WriteLine();
            }
        }
    }
}
