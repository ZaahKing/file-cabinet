using System;

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
                    var index = Array.FindIndex(HelpData.HelpMessages, 0, HelpData.HelpMessages.Length, i => string.Equals(i[HelpData.CommandHelpIndex], commandRequest.Parameters, StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                        Console.WriteLine(HelpData.HelpMessages[index][HelpData.ExplanationHelpIndex]);
                    }
                    else
                    {
                        Console.WriteLine($"There is no explanation for '{commandRequest.Parameters}' command.");
                    }
                }
                else
                {
                    Console.WriteLine("Available commands:");

                    foreach (var helpMessage in HelpData.HelpMessages)
                    {
                        Console.WriteLine("\t{0}\t- {1}", helpMessage[HelpData.CommandHelpIndex], helpMessage[HelpData.DescriptionHelpIndex]);
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
