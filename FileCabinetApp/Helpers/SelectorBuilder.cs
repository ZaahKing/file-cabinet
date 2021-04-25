using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// HElper to select field from record.
    /// </summary>
    internal static class SelectorBuilder
    {
        private static readonly Dictionary<string, Func<FileCabinetRecord, string>> SelectFunctions = new (StringComparer.CurrentCultureIgnoreCase)
        {
            { "id", x => x.Id.ToString() },
            { "firstname", x => x.FirstName },
            { "lastname", x => x.LastName },
            { "dateofbirth", x => x.DateOfBirth.ToString("yyyy-mmm-dd") },
            { "digitkey", x => x.DigitKey.ToString() },
            { "account", x => x.Account.ToString() },
            { "sex", x => $"{x.Sex}" },
        };

        /// <summary>
        /// GEt field value.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Function which returns field value.</returns>
        public static string GetFieldValue(this FileCabinetRecord record, string fieldName)
        {
            return SelectFunctions[fieldName](record);
        }

        /// <summary>
        /// Get list of fields.
        /// </summary>
        /// <returns>List of fields.</returns>
        public static IEnumerable<string> GetFieldsNames()
        {
            return SelectFunctions.Keys;
        }
    }
}
