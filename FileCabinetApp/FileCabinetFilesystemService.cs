﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for filesytem filecabinet data.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private const int FileCabinetRecordSize = 277;
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private readonly BinaryReader reader;
        private readonly BinaryWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="validator">Validator.</param>
        public FileCabinetFilesystemService(FileStream stream, IRecordValidator validator)
        {
            this.fileStream = stream;
            this.validator = validator;
            this.reader = new BinaryReader(stream);
            this.writer = new BinaryWriter(stream);
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
                    record.Id = this.reader.ReadInt32() + 1;
                    this.fileStream.Seek(this.fileStream.Length, SeekOrigin.Begin);
                }
                catch (IOException e)
                {
                    throw new IOException("Cant read from file.", e);
                }
            }

            try
            {
                this.writer.Write((short)0);
                this.writer.Write(record.Id);
                this.WriteRecordWithoutIDInCurrentPosition(record);
                this.writer.Flush();
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
            this.fileStream.Seek(0, SeekOrigin.Begin);
            this.fileStream.Seek(2, SeekOrigin.Current);
            int offset = FileCabinetRecordSize - 4;
            do
            {
                int id = this.reader.ReadInt32();
                if (id == record.Id)
                {
                    this.WriteRecordWithoutIDInCurrentPosition(record);
                    this.writer.Flush();
                    this.fileStream.Seek(0, SeekOrigin.Begin);
                    this.fileStream.Flush();
                    return;
                }

                this.fileStream.Seek(offset, SeekOrigin.Current);
            }
            while (this.fileStream.Position + offset < this.fileStream.Length);
            this.fileStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Helps to find record by date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>FileCabinetRecord. </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByBirthDate(DateTime date)
        {
            var list = this.GetRecordsYield().Where(x => x.DateOfBirth == date).ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Helps to find record by first name.
        /// </summary>
        /// <param name="firstName">FirstName.</param>
        /// <returns>FileCabinetRecord. </returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var list = this.GetRecordsYield().Where(x => x.FirstName.Equals(firstName, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Helps to find record by last name.
        /// </summary>
        /// <param name="lastName">FirstName.</param>
        /// <returns>FileCabinetRecords. </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var list = this.GetRecordsYield().Where(x => x.LastName.Equals(lastName, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Helps to find record by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>FileCabinetRecord. </returns>
        public FileCabinetRecord FindRecordById(int id)
        {
            return this.GetRecordsYield().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Returns all FileCabinetRecords.
        /// </summary>
        /// <returns>Array of FileCabinetRecords.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var list = this.GetRecordsYield().ToList();
            return new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Returns count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / FileCabinetRecordSize);
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
            var list = this.GetRecordsYield().ToArray();
            return new FileCabinetServiceSnapshot(list);
        }

        /// <summary>
        /// Restore file cabinet records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot object.</param>
        /// <returns>Cout of added records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int errorsCount = 0;
            this.fileStream.SetLength(0);
            this.fileStream.Flush();
            var list = snapshot.GetRecords();
            foreach (var record in list)
            {
                try
                {
                    this.CreateRecord(record);
                }
                catch (Exception e)
                {
                    errorsCount++;
                    Console.WriteLine(e.Message);
                }
            }

            return list.Count - errorsCount;
        }

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Id for delation.</param>
        public void RemoveRecord(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern implementation.
        /// </summary>
        /// <param name="disposed">Disposed.</param>
        protected virtual void Dispose(bool disposed)
        {
            this.reader?.Dispose();
            this.writer?.Dispose();
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

        private IEnumerable<FileCabinetRecord> GetRecordsYield()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            while (this.fileStream.Position < this.fileStream.Length)
            {
                var record = new FileCabinetRecord();
                this.fileStream.Seek(2, SeekOrigin.Current);
                record.Id = this.reader.ReadInt32();
                record.FirstName = new string(this.reader.ReadChars(120)).TrimEnd('\0');
                record.LastName = new string(this.reader.ReadChars(120)).TrimEnd('\0');
                record.DateOfBirth = new DateTime(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
                record.DigitKey = this.reader.ReadInt16();
                record.Account = this.reader.ReadDecimal();
                record.Sex = this.reader.ReadChar();
                yield return record;
            }

            this.fileStream.Flush();
            this.fileStream.Seek(0, SeekOrigin.Begin);
        }

        private void WriteRecordWithoutIDInCurrentPosition(FileCabinetRecord record)
        {
            char[] buffer = this.StringToChars(record.FirstName, 120);
            this.writer.Write(buffer);
            buffer = this.StringToChars(record.LastName, 120);
            this.writer.Write(buffer);
            this.writer.Write(record.DateOfBirth.Year);
            this.writer.Write(record.DateOfBirth.Month);
            this.writer.Write(record.DateOfBirth.Day);
            this.writer.Write(record.DigitKey);
            this.writer.Write(record.Account);
            this.writer.Write(record.Sex);
        }
    }
}
