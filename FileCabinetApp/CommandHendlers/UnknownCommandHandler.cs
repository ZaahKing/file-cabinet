using System;
using System.Linq;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Unknown.
    /// </summary>
    internal class UnknownCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (HelpData.HelpMessages.Any(x => commandRequest.Command.Equals(x[HelpData.CommandHelpIndex], StringComparison.CurrentCultureIgnoreCase)))
            {
                this.NextHandler?.Handle(commandRequest);
            }
            else
            {
                Console.WriteLine($"There is no '{commandRequest.Command}' command. Use 'help' command.");
                var list = HelpData.HelpMessages.Select(x => x[HelpData.CommandHelpIndex]).Where(x => x.StartsWith(commandRequest.Command)).ToList();

                if (list.Count == 1)
                {
                    Console.WriteLine($"The most similar command is");
                }
                else if (list.Count > 1)
                {
                    Console.WriteLine($"The most similar commands are");
                }

                foreach (var command in list)
                {
                    Console.WriteLine($"\t{command}");
                }
            }
        }
    }
}
