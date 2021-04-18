using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Account validator.
    /// </summary>
    public class AccountValidator : IRecordValidator
    {
        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (record.Account < 0)
            {
                throw new ArgumentException("Account can't be negative");
            }
        }
    }
}
