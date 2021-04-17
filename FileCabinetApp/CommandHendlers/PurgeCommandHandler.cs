using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Purge command.
    /// </summary>
    internal class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "purge";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            int recordCount = this.Service.GetStat();
            Console.WriteLine($"Data storage processing is completed: {this.Service.PurgeStorage()} of {recordCount} records were purged.");
        }
    }
}
