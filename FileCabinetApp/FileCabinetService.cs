using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly ICollection<FileCabinetRecord> list;
        private readonly FileCabinetRecordValidator validator = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new (StringComparer.CurrentCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new (StringComparer.CurrentCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> bithdayDictionary = new ();

        public FileCabinetService(IFileCabinetGateway gateway)
        {
            this.list = gateway.GetFileCabinetRecords();
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
            this.AddIndex(this.firstNameDictionary, firstName, record);
            this.AddIndex(this.lastNameDictionary, lastName, record);
            this.AddIndex(this.bithdayDictionary, dateOfBirth, record);

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
            var oldRecord = new FileCabinetRecord
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                DigitKey = record.DigitKey,
                Account = record.Account,
                Sex = record.Sex,
            };
            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.DigitKey = digitKey;
            record.Account = account;
            record.Sex = sex;

            this.ChangeIndex(this.firstNameDictionary, oldRecord.FirstName, firstName, record);
            this.ChangeIndex(this.lastNameDictionary, oldRecord.LastName, lastName, record);
            this.ChangeIndex(this.bithdayDictionary, oldRecord.DateOfBirth, dateOfBirth, record);
        }

        public FileCabinetRecord FindRecordById(int id)
        {
            return this.list.FirstOrDefault(x => x.Id == id);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return this.lastNameDictionary[lastName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByBirthDate(DateTime date)
        {
            if (this.bithdayDictionary.ContainsKey(date))
            {
                return this.bithdayDictionary[date].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
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

        private void AddIndex<TDictionary, TKey>(TDictionary dictinary, TKey key, FileCabinetRecord record)
            where TDictionary : Dictionary<TKey, List<FileCabinetRecord>>
        {
            if (!dictinary.ContainsKey(key))
            {
                dictinary.Add(key, new List<FileCabinetRecord>());
            }

            dictinary[key].Add(record);
        }

        private void RemoveIndex<TDictionary, TKey>(TDictionary dictinary, TKey key, FileCabinetRecord record)
            where TDictionary : Dictionary<TKey, List<FileCabinetRecord>>
        {
            if (!dictinary.ContainsKey(key) && dictinary[key].Count <= 1)
            {
                dictinary.Remove(key);
            }

            dictinary[key].Remove(record);
        }

        private void ChangeIndex<TDictionary, TKey>(TDictionary dictinary, TKey oldKey, TKey newKey, FileCabinetRecord record)
            where TDictionary : Dictionary<TKey, List<FileCabinetRecord>>
        {
            if (!oldKey.Equals(newKey))
            {
                this.RemoveIndex(dictinary, oldKey, record);
                this.AddIndex(dictinary, newKey, record);
            }
        }
    }
}
