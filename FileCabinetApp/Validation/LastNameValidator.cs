using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Second name validator.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Min length.</param>
        /// <param name="maxLength">Max length.</param>
        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.FirstName))
            {
                throw new ArgumentException("Lasstname is empty.");
            }

            if (!record.FirstName.LengthInRange(this.minLength, this.maxLength))
            {
                throw new ArgumentException("Larstname length not in range.");
            }
        }
    }
}
