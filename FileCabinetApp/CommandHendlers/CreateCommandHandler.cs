using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Create record comand.
    /// </summary>
    internal class CreateCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public CreateCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("create", StringComparison.CurrentCultureIgnoreCase))
            {
                int id = this.service.CreateRecord(Program.GetFileCabinetRecordFromOutput());
                Console.WriteLine($"Record #{id} is created.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
