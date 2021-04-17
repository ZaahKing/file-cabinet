using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Help.
    /// </summary>
    internal class HelpCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("help", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!string.IsNullOrEmpty(commandRequest.Parameters))
                {
                    var index = Array.FindIndex(Program.HelpMessages, 0, Program.HelpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], commandRequest.Parameters, StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                        Console.WriteLine(Program.HelpMessages[index][Program.ExplanationHelpIndex]);
                    }
                    else
                    {
                        Console.WriteLine($"There is no explanation for '{commandRequest.Parameters}' command.");
                    }
                }
                else
                {
                    Console.WriteLine("Available commands:");

                    foreach (var helpMessage in Program.HelpMessages)
                    {
                        Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                    }
                }

                Console.WriteLine();
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
