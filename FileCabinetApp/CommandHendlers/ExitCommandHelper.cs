using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Exit command.
    /// </summary>
    internal class ExitCommandHelper : CommandHandleBase
    {
        private readonly Action<bool> running;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHelper"/> class.
        /// </summary>
        /// <param name="running">Action for exit.</param>
        public ExitCommandHelper(Action<bool> running)
        {
            this.running = running;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                this.running(false);
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
