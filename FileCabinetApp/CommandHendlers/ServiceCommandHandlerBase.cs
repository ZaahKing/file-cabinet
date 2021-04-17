using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Base command with service.
    /// </summary>
    internal abstract class ServiceCommandHandlerBase : CommandHandleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.Service = service;
        }

        /// <summary>
        /// Gets ets service.
        /// </summary>
        /// <value>
        /// Get or set service.
        /// </value>
        protected IFileCabinetService Service { get; init; }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals(this.GetCommandClue(), StringComparison.CurrentCultureIgnoreCase))
            {
                this.Make(commandRequest);
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }

        /// <summary>
        /// Get clue.
        /// </summary>
        /// <returns>Clue.</returns>
        protected abstract string GetCommandClue();

        /// <summary>
        /// Meake overrided job.
        /// </summary>
        /// <param name="commandRequest">Params.</param>
        protected abstract void Make(AppCommandRequest commandRequest);
    }
}
