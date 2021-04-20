using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.CommandHendlers;
using FileCabinetApp.Validation;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Enter point class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Alexander Belyakoff";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static readonly Dictionary<string, string> SwitchMappings = new ()
            {
                { "-v", "validation" },
                { "--validation-rules", "validation" },
                { "-s", "storage" },
                { "--storage", "storage" },
                { "-u", "stopwatch" },
                { "--use-stopwatch", "stopwatch" },
                { "-l", "logger" },
                { "--use-logger", "logger" },
            };

        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// Programm enter point.
        /// </summary>
        /// <param name="args"> Parameters from consol.</param>
        public static void Main(string[] args)
        {
            var appConfig = new ConfigurationBuilder()
                .AddCommandLine(args, SwitchMappings)
                .Build();

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            string validatorName = DependencyResolver.NormalizeValidatorName(appConfig["validation"]);
            string fileCabinetServicerName = DependencyResolver.NormalizeFileCabinetServiceName(appConfig["storage"]);
            fileCabinetService = DependencyResolver.GetFileCabinetService(fileCabinetServicerName, validatorName);
            Console.WriteLine($"Using {validatorName} validation rules.");
            Console.WriteLine($"Using {fileCabinetServicerName} storage.");

            // Comandline ConfigurationBuilder ignores key whithout value.
            // I deside to add true/false value to commandline for --use-stopwatch.
            if (appConfig.GetSection("stopwatch").Get<bool>())
            {
                fileCabinetService = DependencyResolver.MeterDecorate(fileCabinetService);
                Console.WriteLine("Stopwatch is switched on.");
            }

            if (appConfig.GetSection("logger").Get<bool>())
            {
                fileCabinetService = DependencyResolver.LoggingDecorate(fileCabinetService);
                Console.WriteLine("Logging is switched on.");
            }

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

        private static ICommandHandler CreateCommandsHandlers()
        {
            var empty = new EmptyCommandHandler();
            var unknown = new UnknownCommandHandler();
            var exit = new ExitCommandHelper(x => isRunning = x);
            var help = new HelpCommandHandler();
            var stat = new StatCommandHandler(fileCabinetService);
            var create = new CreateCommandHandler(fileCabinetService, GetFileCabinetRecordFromOutput);
            var edit = new EditCommandHandler(fileCabinetService, GetFileCabinetRecordFromOutput);
            var list = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var find = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
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

        private static FileCabinetRecord GetFileCabinetRecordFromOutput()
        {
            // var validator = fileCabinetService.GetValidator(); // !Impotand
            var record = new FileCabinetRecord
            {
                FirstName = GetOutput(
                    "Firstname: ",
                    () => Console.ReadLine(),
                    x => new FileCabinetRecord { FirstName = x },
                    new FirstNameValidator(2, 60)),

                LastName = GetOutput(
                    "Lastname: ",
                    () => Console.ReadLine(),
                    x => new FileCabinetRecord { LastName = x },
                    new LastNameValidator(2, 60)),

                DateOfBirth = GetOutput(
                    "Day of birth: ",
                    () => DateTime.Parse(Console.ReadLine()),
                    x => new FileCabinetRecord { DateOfBirth = x },
                    new DateOfBirthValidator()),

                DigitKey = GetOutput(
                    "Digit key: ",
                    () => short.Parse(Console.ReadLine()),
                    x => new FileCabinetRecord { DigitKey = x },
                    null),

                Account = GetOutput(
                    "Account value: ",
                    () => decimal.Parse(Console.ReadLine()),
                    x => new FileCabinetRecord { Account = x },
                    null),

                Sex = GetOutput(
                    "Sex: ",
                    () => char.Parse(Console.ReadLine()),
                    x => new FileCabinetRecord { Sex = x },
                    null),
            };
            return record;
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> list)
        {
            int counter = 0;
            foreach (var record in list)
            {
                counter++;
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.DigitKey}, {record.Account}, {record.Sex}");
            }

            if (counter == 0)
            {
                Console.WriteLine("Nothing to display.");
            }
        }

        private static T GetOutput<T>(string message, Func<T> convert, Func<T, FileCabinetRecord> getRecord, IRecordValidator validator)
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
                    validator?.ValidateParameters(getRecord(outputValue));
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
    }
}
