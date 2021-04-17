using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Enter point class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Alexander Belyakoff";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly Tuple<string, Action<string>>[] Commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("purge", Purge),
        };

        private static readonly string[][] HelpMessages = new string[][]
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

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(Commands, 0, Commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    Commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
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

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
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
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
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
            Console.WriteLine($"Data storage processing is completed: {fileCabinetService.PurgeStorage()} of {fileCabinetService.GetStat()} records were purged.");
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
