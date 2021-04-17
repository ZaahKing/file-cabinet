using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (Program.HelpMessages.Any(x => commandRequest.Command.Equals(x[Program.CommandHelpIndex], StringComparison.CurrentCultureIgnoreCase)))
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
