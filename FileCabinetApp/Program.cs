using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.CommandHendlers;

namespace FileCabinetApp
{
    /// <summary>
    /// Enter point class.
    /// </summary>
    public static class Program
    {
        public const int CommandHelpIndex = 0;
        public const int DescriptionHelpIndex = 1;
        public const int ExplanationHelpIndex = 2;

        /// <summary>
        /// Help data.
        /// </summary>
        public static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "create", "create record", "The 'create' command creates a record." },
            new string[] { "edit", "edit record", "The 'edit' command modify a record." },
            new string[] { "list", "print list of records", "The 'list' command prints list of records." },
            new string[] { "stat", "print records count", "The 'stat' command prints records count." },
            new string[] { "find", "find records", "The 'find' command prints records foud by feald and data." },
            new string[] { "remove", "remove records", "The 'remove' command remove record by id." },
            new string[] { "export", "export records to file", "The 'export' command save data to file." },
            new string[] { "import", "import records from file", "The 'import' command load data from file." },
            new string[] { "purge", "purge storage", "The 'purge' command clear storege from unused data." },
        };

        public static IFileCabinetService fileCabinetService;

        private const string DeveloperName = "Alexander Belyakoff";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        /// <summary>
        /// Gets or sets a value indicating whether an exit flag.
        /// </summary>
        /// <value>
        /// False to exit.
        /// </value>
        public static bool IsRunning { get; set; } = true;

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

                var hendler = CreateCommandsHandlers();
                hendler.Handle(request);
            }
            while (IsRunning);
        }

        private static ICommandHandler CreateCommandsHandlers()
        {
            var empty = new EmptyCommandHandler();
            var unknown = new UnknownCommandHandler();
            var exit = new ExitCommandHelper();
            var help = new HelpCommandHandler();
            var stat = new CommandHandler();
            var create = new CommandHandler();
            var edit = new CommandHandler();
            var list = new CommandHandler();
            var find = new CommandHandler();
            var remove = new CommandHandler();
            var export = new CommandHandler();
            var import = new CommandHandler();
            var purge = new CommandHandler();
            purge.SetNext(import);
            import.SetNext(export);
            export.SetNext(remove);
            remove.SetNext(find);
            find.SetNext(list);
            list.SetNext(edit);
            edit.SetNext(create);
            create.SetNext(stat);
            stat.SetNext(help);
            help.SetNext(stat);
            unknown.SetNext(help);
            exit.SetNext(unknown);
            empty.SetNext(exit);
            return empty;
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

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            IsRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s). Including {fileCabinetService.GetStatDeleted()} is ready to purging.");
        }

        private static void Create(string parameters)
        {
            int id = fileCabinetService.CreateRecord(GetFileCabinetRecordFromOutput());
            Console.WriteLine($"Record #{id} is created.");
        }

        private static void Edit(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                Console.WriteLine("Need a numeric parameter.");
                return;
            }

            if (fileCabinetService.FindRecordById(id) is null)
            {
                Console.WriteLine($"Record #{id} is not exist.");
                return;
            }

            var record = GetFileCabinetRecordFromOutput();
            record.Id = id;
            fileCabinetService.EditRecord(record);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void List(string parameters)
        {
            PrintFileCabinetRecordsList(fileCabinetService.GetRecords());
        }

        private static void Find(string parameters)
        {
            (string fieldName, string findKey) = SplitParam(parameters);
            IReadOnlyCollection<FileCabinetRecord> list;
            switch (fieldName)
            {
                case "firstname":
                    {
                        list = fileCabinetService.FindByFirstName(findKey);
                        break;
                    }

                case "lastname":
                    {
                        list = fileCabinetService.FindByLastName(findKey);
                        break;
                    }

                case "dateofbirth":
                    {
                        if (DateTime.TryParse(findKey, out var date))
                        {
                            list = fileCabinetService.FindByBirthDate(date);
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

            PrintFileCabinetRecordsList(list);
        }

        private static void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                Console.WriteLine("Need a numeric parameter.");
                return;
            }

            if (fileCabinetService.FindRecordById(id) is null)
            {
                Console.WriteLine($"Record #{id} is not exist.");
                return;
            }

            fileCabinetService.RemoveRecord(id);
            Console.WriteLine($"Record #{id} is removed.");
        }

        private static void Export(string parameters)
        {
            (string format, string fileName) = SplitParam(parameters);
            if (File.Exists(fileName))
            {
                Console.Write($"File is exist - rewrite {fileName}? [Y/n] ");
                char output = char.ToUpper(Console.ReadLine()[0]);
                if (output != 'Y')
                {
                    return;
                }
            }

            FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
            try
            {
                using (StreamWriter writer = new (fileName))
                {
                    switch (format)
                    {
                        case "csv":
                            snapshot.SaveToCSV(new FileCabinetRecordCsvWriter(writer));
                            break;

                        case "xml":
                            {
                                using FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter = new (writer);
                                snapshot.SaveToXML(fileCabinetRecordXmlWriter);
                                break;
                            }

                        default:
                            Console.WriteLine("Format is not supported.");
                            break;
                    }
                }

                Console.WriteLine($"All records are exported to file {fileName}.");
            }
            catch (IOException)
            {
                Console.WriteLine($"Export failed: can't open file {fileName}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Import(string parameters)
        {
            (string format, string fileName) = SplitParam(parameters);
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File \"{fileName}\"is not exist.");
                return;
            }

            FileStream fileStream;
            try
            {
                fileStream = File.OpenRead(fileName);
            }
            catch (Exception)
            {
                Console.WriteLine("Can't open file");
                return;
            }

            int count = 0;
            var snapshot = new FileCabinetServiceSnapshot();
            switch (format)
            {
                case "csv":
                    snapshot.LoadFromCSV(fileStream);
                    count = fileCabinetService.Restore(snapshot);
                    break;
                case "xml":
                    snapshot.LoadFromXml(fileStream);
                    count = fileCabinetService.Restore(snapshot);
                    break;
                default:
                    Console.WriteLine("Format is not supported.");
                    break;
            }

            Console.WriteLine($"{count} records were imported from {fileName}.");
        }

        private static void Purge(string parameters)
        {
            int recordCount = fileCabinetService.GetStat();
            Console.WriteLine($"Data storage processing is completed: {fileCabinetService.PurgeStorage()} of {recordCount} records were purged.");
        }

        private static (string, string) SplitParam(string parameters)
        {
            string[] args = parameters.Split(' ', 2);
            if (args.Length < 2 || string.IsNullOrWhiteSpace(parameters))
            {
                return (default, default);
            }

            return (args[0].ToLower(), args[1].Trim('"'));
        }

        private static void PrintFileCabinetRecordsList(IReadOnlyCollection<FileCabinetRecord> list)
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

        private static FileCabinetRecord GetFileCabinetRecordFromOutput()
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
    }
}
