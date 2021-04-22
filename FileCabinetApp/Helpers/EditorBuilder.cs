using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Delegate.
    /// </summary>
    /// <param name="record">Record.</param>
    public delegate void Edit(FileCabinetRecord record);

    /// <summary>
    /// Record editor builder.
    /// </summary>
    internal static class EditorBuilder
    {
        private static readonly Dictionary<string, Func<string, Edit>> EditFunctions = new (StringComparer.CurrentCultureIgnoreCase)
        {
            { "id", str => x => x.Id = int.Parse(str) },
            { "firstname", str => x => x.FirstName = str },
            { "lastname", str => x => x.LastName = str },
            { "dateofbirth", str => x => x.DateOfBirth = DateTime.Parse(str) },
            { "digitkey", str => x => x.DigitKey = short.Parse(str) },
            { "account", str => x => x.Account = decimal.Parse(str) },
            { "sex", str => x => x.Sex = str[0] },
        };

        /// <summary>
        /// Get editor.
        /// </summary>
        /// <param name="editPairs">Key/value key.</param>
        /// <returns>Delegate.</returns>
        public static Edit GetRecordEditor(this Dictionary<string, string> editPairs)
        {
            Edit edit = default;

            foreach (var item in editPairs)
            {
                edit += EditFunctions[item.Key](item.Value);
            }

            return edit;
        }
    }
}
