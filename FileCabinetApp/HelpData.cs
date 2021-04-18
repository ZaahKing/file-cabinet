using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain help data.
    /// </summary>
    public static class HelpData
    {
        /// <summary>
        /// Command help index.
        /// </summary>
        public const int CommandHelpIndex = 0;

        /// <summary>
        /// Description index.
        /// </summary>
        public const int DescriptionHelpIndex = 1;

        /// <summary>
        /// Explanation index.
        /// </summary>
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
    }
}
