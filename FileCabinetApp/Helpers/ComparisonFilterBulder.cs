using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Buld comparison filter.
    /// </summary>
    internal static class ComparisonFilterBulder
    {
        private static readonly Dictionary<string, Func<string, Func<FileCabinetRecord, bool>>> ComperisonFilterFunctions = new ()
            {
                { "id", str => x => x.Id == int.Parse(str) },
                { "firstname", str => x => x.FirstName.Equals(str, StringComparison.CurrentCultureIgnoreCase) },
                { "lastname", str => x => x.LastName.Equals(str, StringComparison.CurrentCultureIgnoreCase) },
                { "dateofbirth", str => x => x.DateOfBirth == DateTime.Parse(str) },
                { "digitkey", str => x => x.DigitKey == short.Parse(str) },
                { "account", str => x => x.Account == decimal.Parse(str) },
                { "sex", str => x => x.Sex == str[0] },
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
                list = list.Where(ComperisonFilterFunctions[item.Key](item.Value));
            }

            return list;
        }
    }
}
