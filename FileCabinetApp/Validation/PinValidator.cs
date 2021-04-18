using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Pin validator.
    /// </summary>
    public class PinValidator : IRecordValidator
    {
        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (!record.DigitKey.ValueInRange((short)0, (short)9999))
            {
                throw new ArgumentException("Digit key contains 4 digits only");
            }
        }
    }
}
