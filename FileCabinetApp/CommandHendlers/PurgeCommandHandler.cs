using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Purge command.
    /// </summary>
    internal class PurgeCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public PurgeCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("purge", StringComparison.CurrentCultureIgnoreCase))
            {
                int recordCount = this.service.GetStat();
                Console.WriteLine($"Data storage processing is completed: {this.service.PurgeStorage()} of {recordCount} records were purged.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
