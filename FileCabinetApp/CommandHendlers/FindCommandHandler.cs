using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Find command.
    /// </summary>
    internal class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        /// <param name="printer">Printer.</param>
        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
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

            this.printer(list);
        }
    }
}
