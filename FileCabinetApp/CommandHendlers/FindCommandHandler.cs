using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Find command.
    /// </summary>
    internal class FindCommandHandler : CommandHandleBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public FindCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.Equals("find", StringComparison.CurrentCultureIgnoreCase))
            {
                (string fieldName, string findKey) = CommandHandleBase.SplitParam(commandRequest.Parameters);
                IReadOnlyCollection<FileCabinetRecord> list;
                switch (fieldName)
                {
                    case "firstname":
                        {
                            list = this.service.FindByFirstName(findKey);
                            break;
                        }

                    case "lastname":
                        {
                            list = this.service.FindByLastName(findKey);
                            break;
                        }

                    case "dateofbirth":
                        {
                            if (DateTime.TryParse(findKey, out var date))
                            {
                                list = this.service.FindByBirthDate(date);
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
