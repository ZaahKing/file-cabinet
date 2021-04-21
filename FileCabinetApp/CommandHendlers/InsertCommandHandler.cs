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
    internal class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "insert";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            Console.WriteLine(commandRequest.Parameters);
        }
    }
}
