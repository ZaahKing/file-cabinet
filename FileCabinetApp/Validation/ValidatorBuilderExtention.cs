using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// ValidatorBuilderExtention.
    /// </summary>
    public static class ValidatorBuilderExtention
    {
        /// <summary>
        /// Add validator.
        /// </summary>
        /// <param name="builder">Validator builder.</param>
        /// <param name="validator">Validator to addition.</param>
        /// <returns>Extend validator builder.</returns>
        public static ValidatorBuilder AddAnyValidator(this ValidatorBuilder builder, IRecordValidator validator)
        {
            builder.AddValidator(validator);
            return builder;
        }

        /// <summary>
        /// AddDefaultValidator.
        /// </summary>
        /// <param name="builder">Builder object.</param>
        /// <returns>Default validator.</returns>
        public static ValidatorBuilder AddDefaultValidator(this ValidatorBuilder builder)
        {
            return builder
                .AddAnyValidator(new FirstNameValidator(2, 60))
                .AddAnyValidator(new LastNameValidator(2, 60))
                .AddAnyValidator(new DateOfBirthValidator())
                .AddAnyValidator(new AccountValidator())
                .AddAnyValidator(new PinValidator())
                .AddAnyValidator(new LetterValidator());
        }

        /// <summary>
        /// AddCustomValidator.
        /// </summary>
        /// <param name="builder">Builder object.</param>
        /// <returns>Custom validator.</returns>
        public static ValidatorBuilder AddCustomValidator(this ValidatorBuilder builder)
        {
            return builder
                .AddAnyValidator(new FirstNameValidator(2, 60))
                .AddAnyValidator(new LastNameValidator(4, 60))
                .AddAnyValidator(new DateOfBirthValidator())
                .AddAnyValidator(new AccountValidator())
                .AddAnyValidator(new PinValidator())
                .AddAnyValidator(new ClassicGenderValidator());
        }

        /// <summary>
        /// AddFromConfiguration.
        /// </summary>
        /// <param name="builder">Builder object.</param>
        /// <param name="config">Configuration data.</param>
        /// <returns>Custom validator.</returns>
        public static ValidatorBuilder AddFromConfiguration(this ValidatorBuilder builder, ValidatorConfiguration config)
        {
            return builder
                .AddAnyValidator(new FirstNameValidator(config.Firstname.Min, config.Firstname.Max))
                .AddAnyValidator(new LastNameValidator(config.Lastname.Min, config.Lastname.Max))
                .AddAnyValidator(new DateOfBirthValidator(config.Dateofbirth.From, config.Dateofbirth.To))
                .AddAnyValidator(new PinValidator())
                .AddAnyValidator(new AccountValidator())
                .AddAnyValidator(new ClassicGenderValidator());
        }
    }
}
