using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Create record comand.
    /// </summary>
    internal class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Func<FileCabinetRecord> getOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <param name="getOutput">Get output action.</param>
        public CreateCommandHandler(IFileCabinetService service, Func<FileCabinetRecord> getOutput)
            : base(service)
        {
            this.getOutput = getOutput;
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "create";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            int id = this.Service.CreateRecord(this.getOutput());
            Console.WriteLine($"Record #{id} is created.");
        }
    }
}
