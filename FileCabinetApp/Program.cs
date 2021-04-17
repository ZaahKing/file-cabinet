using System;
using System.Collections.Generic;
using FileCabinetApp.CommandHendlers;

namespace FileCabinetApp
{
    /// <summary>
    /// Enter point class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Alexander Belyakoff";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// Programm enter point.
        /// </summary>
        /// <param name="args"> Parameters from consol.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            string validatorName = DependencyResolver.NormalizeValidatorName(GetComandLiniValueByKey(args, "--validation-rules", "-v"));
            string fileCabinetServicerName = DependencyResolver.NormalizeFileCabinetServiceName(GetComandLiniValueByKey(args, "--storage", "-s"));
            fileCabinetService = DependencyResolver.GetFileCabinetService(fileCabinetServicerName, validatorName);
            Console.WriteLine($"Using {validatorName} validation rules.");
            Console.WriteLine($"Using {fileCabinetServicerName} storage.");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            var hendler = CreateCommandsHandlers();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                const int parametersIndex = 1;
                AppCommandRequest request = new ()
                {
                    Command = inputs[commandIndex],
                    Parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty,
                };

                hendler.Handle(request);
            }
            while (isRunning);
        }

        /// <summary>
        /// Print list of records.
        /// </summary>
        /// <param name="list">List.</param>
        public static void PrintFileCabinetRecordsList(IReadOnlyCollection<FileCabinetRecord> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Nothing to display.");
                return;
            }

            foreach (var record in list)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.DigitKey}, {record.Account}, {record.Sex}");
            }
        }

        /// <summary>
        /// Get output from consol.
        /// </summary>
        /// <returns>Record.</returns>
        public static FileCabinetRecord GetFileCabinetRecordFromOutput()
        {
            var record = new FileCabinetRecord();
            var validator = fileCabinetService.GetValidator();
            record.FirstName = GetOutput("Firstname: ", () => Console.ReadLine(), validator.CheckFirstName);
            record.LastName = GetOutput("Lasttname: ", () => Console.ReadLine(), validator.CheckLastName);
            record.DateOfBirth = GetOutput("Day of birth: ", () => DateTime.Parse(Console.ReadLine()), validator.CheckDateOfBirth);
            record.DigitKey = GetOutput("Digit key: ", () => short.Parse(Console.ReadLine()), validator.CheckDigitKey);
            record.Account = GetOutput("Account value: ", () => decimal.Parse(Console.ReadLine()), validator.CheckAccount);
            record.Sex = GetOutput("Sex: ", () => char.Parse(Console.ReadLine()), validator.CheckSex);
            return record;
        }

        private static ICommandHandler CreateCommandsHandlers()
        {
            var empty = new EmptyCommandHandler();
            var unknown = new UnknownCommandHandler();
            var exit = new ExitCommandHelper(x => isRunning = x);
            var help = new HelpCommandHandler();
            var stat = new StatCommandHandler(fileCabinetService);
            var create = new CreateCommandHandler(fileCabinetService);
            var edit = new EditCommandHandler(fileCabinetService);
            var list = new ListCommandHandler(fileCabinetService);
            var find = new FindCommandHandler(fileCabinetService);
            var remove = new RemoveCommandHandler(fileCabinetService);
            var export = new ExportCommandHandler(fileCabinetService);
            var import = new ImportCommandHandler(fileCabinetService);
            var purge = new PurgeCommandHandler(fileCabinetService);
            purge.SetNext(null);
            import.SetNext(purge);
            export.SetNext(import);
            remove.SetNext(export);
            find.SetNext(remove);
            list.SetNext(find);
            edit.SetNext(list);
            create.SetNext(edit);
            stat.SetNext(create);
            help.SetNext(stat);
            unknown.SetNext(help);
            exit.SetNext(unknown);
            empty.SetNext(exit);
            return empty;
        }

        private static T GetOutput<T>(string message, Func<T> convert, Action<T> validation)
        {
            bool hasErrors;
            T outputValue = default(T);
            do
            {
                try
                {
                    hasErrors = false;
                    Console.Write(message);
                    outputValue = convert();
                    validation(outputValue);
                }
                catch (Exception e)
                {
                    hasErrors = true;
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Try again please.");
                }
            }
            while (hasErrors);
            return outputValue;
        }

        private static string GetComandLiniValueByKey(string[] args, string fullKey, string shortKey)
        {
            string key = default;
            string value = string.Empty;

            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i].StartsWith(shortKey))
                {
                    key = shortKey;
                }
                else if (args[i].StartsWith(fullKey))
                {
                    key = fullKey;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    if (args[i] == key)
                    {
                        value = i + 1 < args.Length ? args[i + 1] : string.Empty;
                    }
                    else
                    {
                        var pair = args[i].Split("=", 2);
                        value = pair.Length == 2 ? pair[1] : string.Empty;
                    }

                    break;
                }
            }

            return value;
        }
    }
}
