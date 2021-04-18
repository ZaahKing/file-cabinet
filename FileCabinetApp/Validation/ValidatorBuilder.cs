using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Validator builder.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new ();

        /// <summary>
        /// Add validator to list.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public void AddValidator(IRecordValidator validator)
        {
            this.validators.Add(validator);
        }

        /// <summary>
        /// Add first name validator to list.
        /// </summary>
        public void ValidateFirstName()
        {
            this.validators.Add(new FirstNameValidator(2, 60));
        }

        /// <summary>
        /// Add last name validator to list.
        /// </summary>
        public void ValidateLastName()
        {
            this.validators.Add(new LastNameValidator(2, 60));
        }

        /// <summary>
        /// Add date of birth validator to list.
        /// </summary>
        public void ValidateDateTIme()
        {
            this.validators.Add(new DateOfBirthValidator());
        }

        /// <summary>
        /// Create validator.
        /// </summary>
        /// <returns>CompositeRecordValidator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeRecordValidator(this.validators);
        }
    }
}
