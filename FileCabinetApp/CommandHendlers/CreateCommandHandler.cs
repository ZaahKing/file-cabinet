using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Create record comand.
    /// </summary>
    internal class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "create";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            int id = this.Service.CreateRecord(Program.GetFileCabinetRecordFromOutput());
            Console.WriteLine($"Record #{id} is created.");
        }
    }
}
