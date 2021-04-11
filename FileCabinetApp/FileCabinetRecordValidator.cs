using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Describe validate methods for FileCabinetRecord fields.
    /// </summary>
    public class FileCabinetRecordValidator
    {
        /// <summary>
        /// Check string is empty or whitespace.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>True if correct.</returns>
        public bool IsNameEmpty(string name) => string.IsNullOrWhiteSpace(name);

        /// <summary>
        /// Check string length.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>False if less 2.</returns>
        public bool IsNameShort(string name) => name.Length < 2;

        /// <summary>
        /// Check string length.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>False if longer 60.</returns>
        public bool IsNameLong(string name) => name.Length > 60;

        /// <summary>
        /// Check name correct.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>True if correct.</returns>
        public bool IsNameCorrect(string name) => !this.IsNameEmpty(name) && !this.IsNameShort(name) && !this.IsNameLong(name);

        /// <summary>
        /// Check date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>False if early 01/01/1950.</returns>
        public bool IsDateOfBirthSmall(DateTime date) => date < new DateTime(1950, 1, 1);

        /// <summary>
        /// Check date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>False if later now.</returns>
        public bool IsDateOfBirthBig(DateTime date) => date > DateTime.Now;

        /// <summary>
        /// Check date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>True if correct.</returns>
        public bool IsDateOfBirthCorrect(DateTime date) => !this.IsDateOfBirthSmall(date) && !this.IsDateOfBirthBig(date);

        /// <summary>
        /// Check id digital key is correct.
        /// </summary>
        /// <param name="key">Digital key.</param>
        /// <returns>True if in range.</returns>
        public bool IsDigitKeyInRange(short key) => key >= 0 && key <= 9999;

        /// <summary>
        /// Check if digital key is correct.
        /// </summary>
        /// <param name="key">Digital key.</param>
        /// <returns>True if correct.</returns>
        public bool IsDigitKeyCorrect(short key) => this.IsDigitKeyInRange(key);

        /// <summary>
        /// Check account possitivity.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <returns>False if negative.</returns>
        public bool IsAccountNegative(decimal account) => account < 0;

        /// <summary>
        /// Check if account is correct.
        /// </summary>
        /// <param name="account">Digital key.</param>
        /// <returns>True if correct.</returns>
        public bool IsAccountCorrect(decimal account) => !this.IsAccountNegative(account);

        /// <summary>
        /// Must be a letter.
        /// </summary>
        /// <param name="sex">Letter.</param>
        /// <returns>True if letter.</returns>
        public bool IsSexLetter(char sex) => char.IsLetter(sex);

        /// <summary>
        /// Must be a letter.
        /// </summary>
        /// <param name="sex">Letter.</param>
        /// <returns>True if correct.</returns>
        public bool IsSexCorrect(char sex) => this.IsSexLetter(sex);

        /// <summary>
        /// All checks for fields.
        /// </summary>
        /// <param name="record">FileCabinetRecord.</param>
        /// <returns>True if correct.</returns>
        public bool IsCorrect(FileCabinetRecord record)
        {
            return this.IsNameCorrect(record.FirstName)
                && this.IsNameCorrect(record.LastName)
                && this.IsDateOfBirthCorrect(record.DateOfBirth)
                && this.IsDigitKeyCorrect(record.DigitKey)
                && this.IsAccountCorrect(record.Account)
                && this.IsSexCorrect(record.Sex);
        }
    }
}
