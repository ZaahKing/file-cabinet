using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for filecabinet data.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private ICollection<FileCabinetRecord> list = new List<FileCabinetRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">Get validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.Validator = validator;
        }

        /// <summary>
        /// Gets validator.
        /// </summary>
        /// <value>Validator.</value>
        public IRecordValidator Validator { get; init; }

        /// <summary>
        /// Add new record.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        /// <returns>Returns Id of new record.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            this.ValidateParameters(record);
            record.Id = (this.list.Count == 0 ? 0 : this.list.Max(x => x.Id)) + 1;
            this.InsertRecord(record);

            return record.Id;
        }

        /// <summary>
        /// Edit existing record found by Id.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        public void EditRecord(FileCabinetRecord record)
        {
            var oldRecord = this.FindRecordById(record.Id);
            if (oldRecord is null)
            {
                throw new ArgumentException($"Record #{record.Id} is not exist.");
            }

            this.ValidateParameters(record);

            this.list.Remove(oldRecord);
            this.list.Add(record);
        }

        /// <inheritdoc/>
        public void InsertRecord(FileCabinetRecord record)
        {
            if (this.list.Any(x => x.Id == record.Id))
            {
                throw new ArgumentException("Record whith same id is already exist.");
            }

            this.list.Add(record);
        }

        /// <summary>
        /// Helps to find record by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>FileCabinetRecord. </returns>
        public FileCabinetRecord FindRecordById(int id)
        {
            return this.list.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Returns all FileCabinetRecords.
        /// </summary>
        /// <returns>Array of FileCabinetRecords.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            return this.list;
        }

        /// <summary>
        /// Returns count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Returns count records for purging.
        /// </summary>
        /// <returns>Count of deleted records.</returns>
        public int GetStatDeleted() => 0;

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Id for delation.</param>
        public void RemoveRecord(int id)
        {
            var record = this.FindRecordById(id);
            if (record is null)
            {
                return;
            }

            this.list.Remove(record);
        }

        /// <summary>
        /// Make file cabinet records snapshot.
        /// </summary>
        /// <returns>FileCabinetServiceSnapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <summary>
        /// Restore file cabinet records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot object.</param>
        /// <returns>Cout of added records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.list = new List<FileCabinetRecord>();
            int successCounter = 0;
            foreach (var record in snapshot.GetRecords())
            {
                try
                {
                    this.Validator.ValidateParameters(record);
                    this.list.Add(record);
                    successCounter++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return successCounter;
        }

        /// <summary>
        /// Purge storage.
        /// </summary>
        /// <returns>Zero becauce inmemroty storage do not need of purging.</returns>
        public int PurgeStorage() => 0;

        /// <summary>
        /// Returns validator used in service.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator GetValidator() => this.Validator;

        /// <summary>
        /// Validation method.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        protected virtual void ValidateParameters(FileCabinetRecord record)
        {
            this.Validator.ValidateParameters(record);
        }
    }
}
