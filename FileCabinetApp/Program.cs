using System;
using System.Collections.Generic;

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

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "create", "create record", "The 'create' command creates a record." },
            new string[] { "edit", "edit record", "The 'edit' command modify a record." },
            new string[] { "list", "print list of records", "The 'list' command prints list of records." },
            new string[] { "stat", "print records count", "The 'stat' command prints records count." },
            new string[] { "find", "find records", "The 'find' command prints records foud by feald and data." },
        };

        /// <summary>
        /// Programm enter point.
        /// </summary>
        /// <param name="args"> Parameters from consol.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            (string serviceName, IRecordValidator validator) = GetFileCabinetValidatorFromCommandLineParams(args);
            fileCabinetService = new FileCabinetService(new FileCabinetMemoryGateway(), validator);
            Console.WriteLine($"Using {serviceName} validation rules.");
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static (string, IRecordValidator) GetFileCabinetValidatorFromCommandLineParams(string[] args)
        {
            string key = default;
            string value = default;
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i].StartsWith("-v"))
                {
                    key = "-v";
                }
                else if (args[i].StartsWith("--validation-rules"))
                {
                    key = "--validation-rules";
                }

                if (!string.IsNullOrEmpty(key))
                {
                    if (args[i] == key)
                    {
                        value = i + 1 < args.Length ? args[i + 1] : default;
                    }
                    else
                    {
                        var pair = args[i].Split("=", 2);
                        value = pair?[1];
                    }

                    break;
                }
            }

            return value?.ToLower() switch
            {
                "custom" => (value, new CustomValidator()),
                _ => ("default", new DefaultValidator()),
            };
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
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
            string[] args = parameters.Split(' ', 2);
            if (args.Length < 2 || string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Wrong parameters count. Format is find [field] \"[key]\"");
                return;
            }

            IReadOnlyCollection<FileCabinetRecord> list;
            string fieldName = args[0].ToLower();
            string findKey = args[1].Trim('"');
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
                        Console.WriteLine("Field is not exist.");
                        return;
                    }
            }

            PrintFileCabinetRecordsList(list);
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
