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
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        public DateOfBirthValidator()
        {
            this.fromDate = new DateTime(1950, 1, 1);
            this.toDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.fromDate = from;
            this.toDate = to;
        }

        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (!record.DateOfBirth.ValueInRange(this.fromDate, this.toDate))
            {
                throw new ArgumentException($"Date of birth can't be earlier {this.fromDate:yyyy-mm-dd} or later {this.toDate:yyyy-mm-dd}");
            }
        }
    }
}
