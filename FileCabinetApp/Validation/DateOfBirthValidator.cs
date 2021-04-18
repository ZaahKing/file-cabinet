using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Date of birth validator.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (!record.DateOfBirth.ValueInRange(new DateTime(1950, 1, 1), DateTime.Now))
            {
                throw new ArgumentException("Date of birth can't be earlier 1950-01-01 or later now");
            }
        }
    }
}
