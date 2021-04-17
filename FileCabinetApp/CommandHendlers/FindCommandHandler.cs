using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Find command.
    /// </summary>
    internal class FindCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public FindCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "find";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            (string fieldName, string findKey) = CommandHandleBase.SplitParam(commandRequest.Parameters);
            IReadOnlyCollection<FileCabinetRecord> list;
            switch (fieldName)
            {
                case "firstname":
                    {
                        list = this.Service.FindByFirstName(findKey);
                        break;
                    }

                case "lastname":
                    {
                        list = this.Service.FindByLastName(findKey);
                        break;
                    }

                case "dateofbirth":
                    {
                        if (DateTime.TryParse(findKey, out var date))
                        {
                            list = this.Service.FindByBirthDate(date);
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
    }
}
