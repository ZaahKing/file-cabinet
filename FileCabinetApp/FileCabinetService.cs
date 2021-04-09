using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly FileCabinetRecordValidator validator = new ();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short digitKey, decimal account, char sex)
        {
            this.MemberValidation(firstName, lastName, dateOfBirth, digitKey, account, sex);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                DigitKey = digitKey,
                Account = account,
                Sex = sex,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public FileCabinetRecordValidator GetValidator() => this.validator;

        private void MemberValidation(string firstName, string lastName, DateTime dateOfBirth, short digitKey, decimal account, char sex)
        {
            if (this.validator.IsNameEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "Firstname can't be null, emty or has only whitespaces");
            }

            if (this.validator.IsNameShort(firstName) && this.validator.IsNameLong(firstName))
            {
                throw new ArgumentException("Firstname can't be less 2 or bigger then 60 letters", nameof(firstName));
            }

            if (this.validator.IsNameEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Lastname can't be null, emty or has only whitespaces");
            }

            if (this.validator.IsNameShort(lastName) || this.validator.IsNameLong(lastName))
            {
                throw new ArgumentException("Lastname can't be less 2 or bigger then 60 letters", nameof(lastName));
            }

            if (this.validator.IsDateOfBirthSmall(dateOfBirth) || this.validator.IsDateOfBirthBig(dateOfBirth))
            {
                throw new ArgumentException("Date of birth can't be earlier 1950-01-01 or later now", nameof(dateOfBirth));
            }

            if (!this.validator.IsDigitKeyInRange(digitKey))
            {
                throw new ArgumentException("Digit key contains 4 digits only", nameof(digitKey));
            }

            if (this.validator.IsAccountNegative(account))
            {
                throw new ArgumentException("Account can't be negative", nameof(account));
            }

            if (!this.validator.IsSexLetter(sex))
            {
                throw new ArgumentException("Sex parabeter has to contain a letter describing a sex", nameof(sex));
            }
        }
    }
}
