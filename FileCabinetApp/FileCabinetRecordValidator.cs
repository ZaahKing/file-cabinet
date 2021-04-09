using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetRecordValidator
    {
        public bool IsNameEmpty(string name) => string.IsNullOrWhiteSpace(name);

        public bool IsNameShort(string name) => name.Length < 2;

        public bool IsNameLong(string name) => name.Length > 60;

        public bool IsNameCorrect(string name) => !this.IsNameEmpty(name) && !this.IsNameShort(name) && !this.IsNameLong(name);

        public bool IsDateOfBirthSmall(DateTime date) => date < new DateTime(1950, 1, 1);

        public bool IsDateOfBirthBig(DateTime date) => date > DateTime.Now;

        public bool IsDateOfBirthCorrect(DateTime date) => !this.IsDateOfBirthSmall(date) && !this.IsDateOfBirthBig(date);

        public bool IsDigitKeyInRange(short key) => key >= 0 && key <= 9999;

        public bool IsDigitKeyCorrect(short key) => this.IsDigitKeyInRange(key);

        public bool IsAccountNegative(decimal account) => account < 0;

        public bool IsAccountCorrect(decimal account) => !this.IsAccountNegative(account);

        public bool IsSexLetter(char sex) => char.IsLetter(sex);

        public bool IsSexCorrect(char sex) => this.IsSexLetter(sex);

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
