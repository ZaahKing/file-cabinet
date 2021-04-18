using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Stat command.
    /// </summary>
    internal class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "stat";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            var recordsCount = this.Service.GetStat();
            Console.WriteLine($"{recordsCount} record(s). Including {this.Service.GetStatDeleted()} is ready to purging.");
        }
    }
}
