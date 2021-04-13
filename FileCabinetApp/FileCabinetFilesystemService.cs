using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for filesytem filecabinet data.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="validator">Validator.</param>
        public FileCabinetFilesystemService(FileStream stream, IRecordValidator validator)
        {
            this.fileStream = stream;
            this.validator = validator;
        }

        /// <summary>
        /// Add new record.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        /// <returns>Returns Id of new record.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit existing record found by Id.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        public void EditRecord(FileCabinetRecord record)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helps to find record by date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>FileCabinetRecord. </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByBirthDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helps to find record by first name.
        /// </summary>
        /// <param name="firstName">FirstName.</param>
        /// <returns>FileCabinetRecord. </returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helps to find record by last name.
        /// </summary>
        /// <param name="lastName">FirstName.</param>
        /// <returns>FileCabinetRecords. </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helps to find record by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>FileCabinetRecord. </returns>
        public FileCabinetRecord FindRecordById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all FileCabinetRecords.
        /// </summary>
        /// <returns>Array of FileCabinetRecords.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns validator used in service.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator GetValidator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Make file cabinet records snapshot.
        /// </summary>
        /// <returns>FileCabinetServiceSnapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
