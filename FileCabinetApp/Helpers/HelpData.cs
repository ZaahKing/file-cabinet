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
            new string[] { "stat", "print records count", "The 'stat' command prints records count." },
            new string[] { "remove", "remove records", "The 'remove' command remove record by id." },
            new string[] { "export", "export records to file", "The 'export' command save data to file." },
            new string[] { "import", "import records from file", "The 'import' command load data from file." },
            new string[] { "purge", "purge storage", "The 'purge' command clear storege from unused data." },
            new string[] { "insert", "insert in the storage", "The 'insert' command insert a record in a storage." },
            new string[] { "delete", "delete in the storage", "The 'delete' command remove records from a storage." },
            new string[] { "update", "updetr a record", "The 'update' command update a record in a storage." },
            new string[] { "select", "select records", "The 'select' print record filtered list." },
        };
    }
}
