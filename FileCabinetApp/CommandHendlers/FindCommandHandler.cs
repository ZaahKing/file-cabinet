using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Find command.
    /// </summary>
    internal class FindCommandHandler : CommandHandleBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("find", StringComparison.CurrentCultureIgnoreCase))
            {
                (string fieldName, string findKey) = Program.SplitParam(commandRequest.Parameters);
                IReadOnlyCollection<FileCabinetRecord> list;
                switch (fieldName)
                {
                    case "firstname":
                        {
                            list = Program.FileCabinetService.FindByFirstName(findKey);
                            break;
                        }

                    case "lastname":
                        {
                            list = Program.FileCabinetService.FindByLastName(findKey);
                            break;
                        }

                    case "dateofbirth":
                        {
                            if (DateTime.TryParse(findKey, out var date))
                            {
                                list = Program.FileCabinetService.FindByBirthDate(date);
                            }
                            else
                            {
                                Console.WriteLine("Date is not correct.");
                                return;
                            }

                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Wrong parameters. Format is find [field] \"[key]\"");
                            return;
                        }
                }

                Program.PrintFileCabinetRecordsList(list);
            }
            else
            {
                this.NextHandler?.Handle(commandRequest);
            }
        }
    }
}
