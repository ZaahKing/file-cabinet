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
        private static readonly char[] DeleationSymbols = new char[] { '\'', ' ' };
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

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim(DeleationSymbols);
                values[i] = values[i].Trim(DeleationSymbols);
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

        /// <summary>
        /// Get where pairs.
        /// </summary>
        /// <param name="section">String section whith key/value pairs.</param>
        /// <returns>Dictionary.</returns>
        public static Dictionary<string, string> GetWherePairs(this string section)
        {
            var list = section.Split("and", StringComparison.CurrentCultureIgnoreCase);
            return SplitKeyAndValue(list);
        }

        /// <summary>
        /// Get set section pairs.
        /// </summary>
        /// <param name="section">String section whith key/value pairs.</param>
        /// <returns>Dictionary.</returns>
        public static Dictionary<string, string> GetSetPairs(this string section)
        {
            var list = section.Split(',');
            return SplitKeyAndValue(list);
        }

        /// <summary>
        /// Split string by string.
        /// </summary>
        /// <param name="source">Trimmed string.</param>
        /// <param name="separator">Separator.</param>
        /// <param name="comparison">Comparison type.</param>
        /// <returns>List.</returns>
        public static IEnumerable<string> Split(this string source, string separator, StringComparison comparison = StringComparison.CurrentCulture)
        {
            var result = new List<string>();

            if (string.IsNullOrWhiteSpace(source))
            {
                return result;
            }

            int previouseIndex = 0, currentIndex = 0;
            do
            {
                currentIndex = source.IndexOf(separator, previouseIndex, comparison);
                if (currentIndex >= 0)
                {
                    result.Add(source[previouseIndex..currentIndex]);
                    previouseIndex = currentIndex + separator.Length;
                }
                else
                {
                    result.Add(source[previouseIndex..^0]);
                }
            }
            while (currentIndex >= 0);

            return result;
        }

        private static Dictionary<string, string> SplitKeyAndValue(IEnumerable<string> list)
        {
            var pairs = new Dictionary<string, string>();
            foreach (var item in list)
            {
                var pair = item.Split('=');
                if (pair.Length != 2)
                {
                    throw new ArgumentException($"'{item}' is not a key/value pair.");
                }

                pairs.Add(pair[0].Trim(DeleationSymbols), pair[1].Trim(DeleationSymbols));
            }

            return pairs;
        }
    }
}
