using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Edit command.
    /// </summary>
    internal class EditCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public EditCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("edit", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!int.TryParse(commandRequest.Parameters, out var id))
                {
                    Console.WriteLine("Need a numeric parameter.");
                    return;
                }

                if (this.service.FindRecordById(id) is null)
                {
                    Console.WriteLine($"Record #{id} is not exist.");
                    return;
                }

                var record = Program.GetFileCabinetRecordFromOutput();
                record.Id = id;
                this.service.EditRecord(record);
                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
