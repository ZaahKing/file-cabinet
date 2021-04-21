using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Insert record command.
    /// </summary>
    internal class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "update";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            Console.WriteLine(commandRequest.Parameters);
        }
    }
}
