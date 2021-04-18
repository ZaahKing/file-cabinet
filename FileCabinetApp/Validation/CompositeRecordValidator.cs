using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Multivalidator.
    /// </summary>
    public class CompositeRecordValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeRecordValidator"/> class.
        /// </summary>
        /// <param name="validators">List of validators.</param>
        public CompositeRecordValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        /// <inheritdoc/>
        public void ValidateParameters(FileCabinetRecord record)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(record);
            }
        }
    }
}
