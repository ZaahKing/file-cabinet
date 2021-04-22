using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Decorator for IFileCabinetService to log in file.
    /// </summary>
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly StreamWriter writer;
        private readonly IFileCabinetService service;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        /// <param name="logFileName">File name.</param>
        public ServiceLogger(IFileCabinetService service, string logFileName)
        {
            this.service = service;
            this.writer = new StreamWriter(File.Open(logFileName, FileMode.Append));
        }

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetRecord record)
        {
            return this.CallMethod(
                $"Calling CreateRecord() with FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth}', Pin = '{record.DigitKey}', Account = '{record.Account}', Sex = '{record.Sex}'",
                $"CreateRecord() returned '{0}'",
                () => this.service.CreateRecord(record));
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord record)
        {
            this.CallMethod(
                $"Calling EditRecord() with Id = {record.Id}, FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth}', Pin = '{record.DigitKey}', Account = '{record.Account}', Sex = '{record.Sex}'",
                $"EditRecord() is success.",
                () => this.service.EditRecord(record));
        }

        /// <inheritdoc/>
        public void InsertRecord(FileCabinetRecord record)
        {
            this.CallMethod(
                $"Calling InsertRecord() with Id = {record.Id}, FirstName = '{record.FirstName}', LastName = '{record.LastName}', DateOfBirth = '{record.DateOfBirth}', Pin = '{record.DigitKey}', Account = '{record.Account}', Sex = '{record.Sex}'",
                $"InsertRecord() is success.",
                () => this.service.InsertRecord(record));
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByBirthDate(DateTime date)
        {
            return this.CallMethodWithCollection(
                $"Calling FindByBirthDate() with date = '{date:yyyy-mm-dd}.",
                $"FindByBirthDate() returned '{0}' lines",
                () => this.service.FindByBirthDate(date));
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.CallMethodWithCollection(
            $"Calling FindByFirstName() with firstname = '{firstName}.",
            $"FindByFirstName() returned '{0}' lines",
            () => this.service.FindByFirstName(firstName));
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.CallMethodWithCollection(
            $"Calling FindByLastName() with lastname = '{lastName}.",
            $"FindByLastName() returned '{0}' lines",
            () => this.service.FindByLastName(lastName));
        }

        /// <inheritdoc/>
        public FileCabinetRecord FindRecordById(int id)
        {
            return this.CallMethod(
            $"Calling FindRecordById() with id = '{id}.",
            $"FindRecordById() is success.",
            () => this.service.FindRecordById(id));
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            return this.CallMethod(
            $"Calling GetRecords().",
            $"GetRecords() returned '{0}' lines",
            () => this.service.GetRecords());
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.CallMethod(
            $"Calling GetStat().",
            $"GetStat() returned '{0}'.",
            () => this.service.GetStat());
        }

        /// <inheritdoc/>
        public int GetStatDeleted()
        {
            return this.CallMethod(
            $"Calling GetStatDeleted().",
            $"GetStatDeleted() returned '{0}'.",
            () => this.service.GetStatDeleted());
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return this.CallMethod(
            $"Calling GetValidator().",
            $"GetValidator() is success.",
            () => this.service.GetValidator());
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return this.CallMethod(
            $"Calling MakeSnapshot().",
            $"MakeSnapshot() is success.",
            () => this.service.MakeSnapshot());
        }

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            return this.CallMethod(
            $"Calling Restore().",
            $"Restore() restores {0} records.",
            () => this.service.Restore(snapshot));
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            this.CallMethod(
            $"Calling RemoveRecord() with Id = {id}",
            $"RemoveRecord() is success.",
            () => this.service.RemoveRecord(id));
        }

        /// <inheritdoc/>
        public int PurgeStorage()
        {
            return this.CallMethod(
            $"Calling PurgeStorage().",
            $"PurgeStorage() purge {0} records.",
            () => this.service.PurgeStorage());
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Disposing flag.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.writer?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private void LogMessage(string message)
        {
            this.writer.WriteLine($"{DateTime.Now:dd/mm/yyyy hh:mm} - {message}");
            this.writer.Flush();
        }

        private TOutput CallMethod<TOutput>(string inMessage, string outMessage, Func<TOutput> loggededAction)
        {
            this.LogMessage(inMessage);
            TOutput result = loggededAction();
            this.LogMessage(string.Format(outMessage, result));
            return result;
        }

        private void CallMethod(string inMessage, string outMessage, Action loggededAction)
        {
            this.LogMessage(inMessage);
            loggededAction();
            this.LogMessage(outMessage);
        }

        private TOutput CallMethodWithCollection<TOutput>(string inMessage, string outMessage, Func<TOutput> loggededAction)
            where TOutput : IEnumerable<FileCabinetRecord>
        {
            this.LogMessage(inMessage);
            TOutput result = loggededAction();
            this.LogMessage(string.Format(outMessage, result.Count()));
            return result;
        }
    }
}
