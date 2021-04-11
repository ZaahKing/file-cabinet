using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Default file record validator.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Check is account correct.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <exception cref="ArgumentException">When account is negative.</exception>
        public void CheckAccount(decimal account)
        {
            if (account < 0)
            {
                throw new ArgumentException("Account can't be negative", nameof(account));
            }
        }

        /// <summary>
        /// Check is date of birth correct.
        /// </summary>
        /// <param name="dateofBirth">Date of birth.</param>
        /// <exception cref="ArgumentException">When date is out of range.</exception>
        public void CheckDateOfBirth(DateTime dateofBirth)
        {
            if (dateofBirth > DateTime.Now || dateofBirth < new DateTime(1950, 1, 1))
            {
                throw new ArgumentException("Date of birth can't be earlier 1950-01-01 or later now", nameof(dateofBirth));
            }
        }

        /// <summary>
        /// Check is digit key correct.
        /// </summary>
        /// <param name="digitKey">Digital key.</param>
        /// <exception cref="ArgumentException">When key is not 4 digits.</exception>
        public void CheckDigitKey(short digitKey)
        {
            if (digitKey < 0 || digitKey > 9999)
            {
                throw new ArgumentException("Digit key contains 4 digits only", nameof(digitKey));
            }
        }

        /// <summary>
        /// Check is firstname correct.
        /// </summary>
        /// <param name="firstName">Firstname.</param>
        /// <exception cref="ArgumentNullException">When name is empty.</exception>
        /// <exception cref="ArgumentException">When length less 2 or bigger then 60 letters.</exception>
        public void CheckFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "Firstname can't be null, emty or has only whitespaces");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("Firstname can't be less 2 or bigger then 60 letters", nameof(firstName));
            }
        }

        /// <summary>
        /// Check is lastname correct.
        /// </summary>
        /// <param name="lastName">Lastname.</param>
        /// <exception cref="ArgumentNullException">When name is empty.</exception>
        /// <exception cref="ArgumentException">When length less 2 or bigger then 60 letters.</exception>
        public void CheckLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Firstname can't be null, emty or has only whitespaces");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Firstname can't be less 2 or bigger then 60 letters", nameof(lastName));
            }
        }

        /// <summary>
        /// Check is sex correct.
        /// </summary>
        /// <param name="sex">Sex.</param>
        /// <exception cref="ArgumentException">When is not a letter.</exception>
        public void CheckSex(char sex)
        {
            if (!char.IsLetter(sex))
            {
                throw new ArgumentException("Sex parabeter has to contain a letter describing a sex", nameof(sex));
            }
        }
    }
}
