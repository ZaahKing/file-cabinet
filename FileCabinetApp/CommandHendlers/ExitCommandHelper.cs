﻿using System;

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
