using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Exit command.
    /// </summary>
    internal class ExitCommandHelper : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Program.IsRunning = false;
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
