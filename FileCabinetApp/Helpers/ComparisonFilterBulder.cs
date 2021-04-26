using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Buld comparison filter.
    /// </summary>
    internal static class ComparisonFilterBulder
    {
        private static readonly Dictionary<string, Func<string, Func<IComparable, IComparable, bool>, Func<FileCabinetRecord, bool>>> ComperisonFilterFunctions = new ()
            {
                { "id", (str, copareFunc) => x => copareFunc(x.Id, int.Parse(str)) },
                { "firstname", (str, copareFunc) => x => copareFunc(x.FirstName, str) },
                { "lastname", (str, copareFunc) => x => copareFunc(x.LastName, str) },
                { "dateofbirth", (str, copareFunc) => x => copareFunc(x.DateOfBirth, DateTime.Parse(str)) },
                { "digitkey", (str, copareFunc) => x => copareFunc(x.DigitKey, short.Parse(str)) },
                { "account", (str, copareFunc) => x => copareFunc(x.Account, decimal.Parse(str)) },
                { "sex", (str, copareFunc) => x => copareFunc(x.Sex, str[0]) },
            };

        /// <summary>
        /// Get Filtered List.
        /// </summary>
        /// <param name="list">Source list.</param>
        /// <param name="compariconPairs">compariconPairs.</param>
        /// <returns>Enumarable with filters.</returns>
        public static IEnumerable<FileCabinetRecord> GetFilteredList(this IEnumerable<FileCabinetRecord> list, Dictionary<string, string> compariconPairs)
        {
            foreach (var item in compariconPairs)
            {
                list = list.Where(ComperisonFilterFunctions[item.Key](item.Value, (a, b) => a.CompareTo(b) == 0));
            }

            return list;
        }

        /// <summary>
        /// GetComterisonFunction.
        /// </summary>
        /// <param name="key">Key of record.</param>
        /// <param name="value">Value to compare.</param>
        /// <param name="compareFunc">Compare function.</param>
        /// <returns>Result.</returns>
        public static Func<FileCabinetRecord, bool> GetComterisonFunction(string key, string value, Func<IComparable, IComparable, bool> compareFunc)
        {
            return ComperisonFilterFunctions[key](value, compareFunc);
        }
    }
}
