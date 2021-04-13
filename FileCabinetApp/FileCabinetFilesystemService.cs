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
        private const int FileCabinetRecordSize = 277;
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
            this.validator.CheckAll(record);
            if (this.fileStream.Length == 0)
            {
                record.Id = 1;
            }
            else
            {
                try
                {
                    this.fileStream.Seek(this.fileStream.Length - 275, SeekOrigin.Begin);
                    var reader = new BinaryReader(this.fileStream);
                    record.Id = reader.ReadInt32() + 1;
                    this.fileStream.Seek(this.fileStream.Length, SeekOrigin.Begin);
                }
                catch (IOException e)
                {
                    throw new IOException("Cant read from file.", e);
                }
            }

            try
            {
                BinaryWriter writer = new BinaryWriter(this.fileStream);
                writer.Write((short)0);
                writer.Write(record.Id);
                char[] buffer = this.StringToChars(record.FirstName, 120);
                writer.Write(buffer);
                buffer = this.StringToChars(record.LastName, 120);
                writer.Write(buffer);
                writer.Write(record.DateOfBirth.Year);
                writer.Write(record.DateOfBirth.Month);
                writer.Write(record.DateOfBirth.Day);
                writer.Write(record.DigitKey);
                writer.Write(record.Account);
                writer.Write(record.Sex);
                writer.Flush();
            }
            catch (IOException e)
            {
                throw new IOException("Cant write to file.", e);
            }

            return record.Id;
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
            List<FileCabinetRecord> list = new ();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            var reader = new BinaryReader(this.fileStream);
            while (this.fileStream.Position < this.fileStream.Length)
            {
                var record = new FileCabinetRecord();
                this.fileStream.Seek(2, SeekOrigin.Current);
                record.Id = reader.ReadInt32();
                record.FirstName = new string(reader.ReadChars(120)).TrimEnd('\0');
                record.LastName = new string(reader.ReadChars(120)).TrimEnd('\0');
                record.DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                record.DigitKey = reader.ReadInt16();
                record.Account = reader.ReadDecimal();
                record.Sex = reader.ReadChar();
                list.Add(record);
            }

            this.fileStream.Flush();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            return new ReadOnlyCollection<FileCabinetRecord>(list);
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
            return this.validator;
        }

        /// <summary>
        /// Make file cabinet records snapshot.
        /// </summary>
        /// <returns>FileCabinetServiceSnapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        private char[] StringToChars(string data, int arrayLength)
        {
            char[] result = new char[arrayLength];
            for (int i = 0; i < data.Length; ++i)
            {
                result[i] = data[i];
            }

            return result;
        }
    }
}
