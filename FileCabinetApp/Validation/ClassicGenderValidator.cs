using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Classic gender validator.
    /// </summary>
    public class ClassicGenderValidator : IRecordValidator
    {
        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (!record.Sex.IsClassicGender())
            {
                throw new ArgumentException("Sex parabeter has to contain a letter describing a sex");
            }
        }
    }
}
