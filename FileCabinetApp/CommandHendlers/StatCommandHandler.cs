using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Stat command.
    /// </summary>
    internal class StatCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public StatCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("stat", StringComparison.CurrentCultureIgnoreCase))
            {
                var recordsCount = this.service.GetStat();
                Console.WriteLine($"{recordsCount} record(s). Including {this.service.GetStatDeleted()} is ready to purging.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
