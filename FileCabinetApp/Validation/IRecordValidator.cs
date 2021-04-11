using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet record validation interface.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Check is firstname correct.
        /// </summary>
        /// <param name="firstName">Firstname.</param>
        void CheckFirstName(string firstName);

        /// <summary>
        /// Check is lastname correct.
        /// </summary>
        /// <param name="lastName">Lastname.</param>
        void CheckLastName(string lastName);

        /// <summary>
        /// Check is date of birth correct.
        /// </summary>
        /// <param name="dateofBirth">Date of birth.</param>
        void CheckDateOfBirth(DateTime dateofBirth);

        /// <summary>
        /// Check is digit key correct.
        /// </summary>
        /// <param name="digitKey">Digital key.</param>
        void CheckDigitKey(short digitKey);

        /// <summary>
        /// Check is account correct.
        /// </summary>
        /// <param name="account">Account.</param>
        void CheckAccount(decimal account);

        /// <summary>
        /// Check is sex correct.
        /// </summary>
        /// <param name="sex">Sex.</param>
        void CheckSex(char sex);

        /// <summary>
        /// Check is file cabinet record correct.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        void CheckAll(FileCabinetRecord record)
        {
            this.CheckFirstName(record.FirstName);
            this.CheckLastName(record.LastName);
            this.CheckDateOfBirth(record.DateOfBirth);
            this.CheckDigitKey(record.DigitKey);
            this.CheckAccount(record.Account);
            this.CheckSex(record.Sex);
        }
    }
}
