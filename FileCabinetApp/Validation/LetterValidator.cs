using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Sex letter validator.
    /// </summary>
    public class LetterValidator : IRecordValidator
    {
        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (!char.IsLetter(record.Sex))
            {
                throw new ArgumentException("Sex parabeter has to contain a letter describing a sex");
            }
        }
    }
}
