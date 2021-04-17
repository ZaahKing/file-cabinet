using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Edit command.
    /// </summary>
    internal class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public EditCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "edit";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            if (!int.TryParse(commandRequest.Parameters, out var id))
            {
                Console.WriteLine("Need a numeric parameter.");
                return;
            }

            if (this.Service.FindRecordById(id) is null)
            {
                Console.WriteLine($"Record #{id} is not exist.");
                return;
            }

            var record = Program.GetFileCabinetRecordFromOutput();
            record.Id = id;
            this.Service.EditRecord(record);
            Console.WriteLine($"Record #{id} is updated.");
        }
    }
}
