using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Remove record command.
    /// </summary>
    internal class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "remove";

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

            this.Service.RemoveRecord(id);
            Console.WriteLine($"Record #{id} is removed.");
        }
    }
}
