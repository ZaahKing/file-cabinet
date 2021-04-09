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

        public FileCabinetService()
        {
            this.CreateRecord("Alex", "Whiter", new DateTime(1982, 12, 6), 1234, 45m, 'm');
            this.CreateRecord("Alex", "Booter", new DateTime(1990, 10, 10), 1234, 45m, 'm');
            this.CreateRecord("Xena", "Queen", new DateTime(1982, 12, 6), 1234, 45m, 'f');
            this.CreateRecord("Anastatia", "Queen", new DateTime(1982, 12, 6), 1234, 45m, 'f');
        }

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

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short digitKey, decimal account, char sex)
        {
            var record = this.FindRecordById(id);
            if (record is null)
            {
                throw new ArgumentException($"Record #{id} is not exist.");
            }

            this.MemberValidation(firstName, lastName, dateOfBirth, digitKey, account, sex);
            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.DigitKey = digitKey;
            record.Account = account;
            record.Sex = sex;
        }

        public FileCabinetRecord FindRecordById(int id)
        {
            return this.list.FirstOrDefault(x => x.Id == id);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.list.Where(x => x.FirstName.Equals(firstName, StringComparison.CurrentCultureIgnoreCase)).ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string firstName)
        {
            return this.list.Where(x => x.LastName.Equals(firstName, StringComparison.CurrentCultureIgnoreCase)).ToArray();
        }

        public FileCabinetRecord[] FindByBirthDate(DateTime date)
        {
            return this.list.Where(x => x.DateOfBirth == date).ToArray();
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
