using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Just helper for parse.
    /// I choose an esier way whithout strong rules for time economy.
    /// </summary>
    internal static class CommandParameterParser
    {
        private static readonly Dictionary<string, Action<FileCabinetRecord, string>> ConvertRules = new (StringComparer.CurrentCultureIgnoreCase)
        {
            { "id", (record, param) => { record.Id = int.Parse(param); } },
            { "firstname", (record, param) => { record.FirstName = param; } },
            { "lastname", (record, param) => { record.LastName = param; } },
            { "dateofbirth", (record, param) => { record.DateOfBirth = DateTime.Parse(param); } },
            { "digitkey", (record, param) => { record.DigitKey = short.Parse(param); } },
            { "account", (record, param) => { record.Account = decimal.Parse(param); } },
            { "sex", (record, param) => { record.Sex = param[0]; } },
        };

        /// <summary>
        /// Get record.
        /// </summary>
        /// <param name="parameters">Just string.</param>
        /// <returns>File cabinet record.</returns>
        public static FileCabinetRecord GetRecordFromInsertCommandParameter(this string parameters)
        {
            var seekLine = " values ";
            int seekPosition = parameters.IndexOf(seekLine, StringComparison.CurrentCultureIgnoreCase);
            if (seekPosition < 0)
            {
                throw new ArgumentException("Value section is not exist.");
            }

            var breakets = new char[] { '(', ')' };
            var fields = parameters[0..seekPosition].Trim(breakets).Split(',');
            var values = parameters[(seekPosition + seekLine.Length) .. ^0].Trim(breakets).Split(',');
            if (fields.Length != 7 || values.Length != 7)
            {
                throw new ArgumentException("Fields and values count must be 7.");
            }

            var deleationSymbols = new char[] { '\'', ' ' };
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim(deleationSymbols);
                values[i] = values[i].Trim(deleationSymbols);
            }

            if (!ConvertRules.Keys.All(x => fields.Any(y => y.Equals(x, StringComparison.CurrentCultureIgnoreCase))))
            {
                throw new ArgumentException("File cabinet records contain only 'id', 'firstname', 'lastname', 'dateofbirth', 'digitkey', 'account', 'sex' fields.");
            }

            var result = new FileCabinetRecord();
            for (int i = 0; i < fields.Length; i++)
            {
                ConvertRules[fields[i]](result, values[i]);
            }

            return result;
        }
    }
}
