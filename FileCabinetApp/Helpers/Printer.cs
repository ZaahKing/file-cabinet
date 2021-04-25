using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Print FileCabinetRecords list.
    /// </summary>
    public class Printer
    {
        private static readonly Dictionary<string, int> NeededLength = new (StringComparer.CurrentCultureIgnoreCase)
        {
            { "id", 4 },
            { "firstname", -20 },
            { "lastname", -20 },
            { "dateofbirth", 11 },
            { "digitkey", 4 },
            { "account", 10 },
            { "sex", 1 },
        };

        private readonly IEnumerable<string> fields;
        private readonly string delimeterLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Printer"/> class.
        /// </summary>
        /// <param name="fields">Printable fields.</param>
        public Printer(IEnumerable<string> fields)
        {
            this.fields = fields;
            StringBuilder line = new ("+");
            foreach (var field in fields)
            {
                line.Append('-', Math.Abs(NeededLength[field]) + 2);
                line.Append('+');
            }

            this.delimeterLine = line.ToString();
        }

        /// <summary>
        /// Print method.
        /// </summary>
        /// <param name="list">List for print.</param>
        public void Print(IEnumerable<FileCabinetRecord> list)
        {
            this.PrintHeader();
            int counter = 0;
            foreach (var record in list)
            {
                Console.Write("| ");
                counter++;
                foreach (var fieldName in this.fields)
                {
                    Console.Write(FillString(record.GetFieldValue(fieldName), NeededLength[fieldName]) + " | ");
                }

                Console.WriteLine();
                Console.WriteLine(this.delimeterLine);
            }

            if (counter == 0)
            {
                Console.WriteLine("Nothing to display.");
            }
        }

        private static string FillString(string value, int neededLength)
        {
            int absNeededLength = Math.Abs(neededLength);
            if (value.Length >= absNeededLength)
            {
                return value.Substring(0, absNeededLength);
            }

            string fillString = new (' ', absNeededLength - value.Length);
            if (neededLength < 0)
            {
                return $"{value}{fillString}";
            }
            else
            {
                return $"{fillString}{value}";
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine(this.delimeterLine);
            Console.Write("| ");
            foreach (var fieldName in this.fields)
            {
                Console.Write(FillString(fieldName, NeededLength[fieldName]) + " | ");
            }

            Console.WriteLine();
            Console.WriteLine(this.delimeterLine);
        }
    }
}
