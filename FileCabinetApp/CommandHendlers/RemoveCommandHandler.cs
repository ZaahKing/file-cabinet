using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Remove record command.
    /// </summary>
    internal class RemoveCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public RemoveCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("remove", StringComparison.CurrentCultureIgnoreCase))
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

                this.service.RemoveRecord(id);
                Console.WriteLine($"Record #{id} is removed.");
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
