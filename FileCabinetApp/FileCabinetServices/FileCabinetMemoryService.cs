﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for filecabinet data.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new (StringComparer.CurrentCultureIgnoreCase);
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new (StringComparer.CurrentCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> bithdayDictionary = new ();
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
            this.list.Add(record);
            this.AddIndex(this.firstNameDictionary, record.FirstName, record);
            this.AddIndex(this.lastNameDictionary, record.LastName, record);
            this.AddIndex(this.bithdayDictionary, record.DateOfBirth, record);

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

            this.ChangeIndex(this.firstNameDictionary, oldRecord.FirstName, record.FirstName, record);
            this.ChangeIndex(this.lastNameDictionary, oldRecord.LastName, record.LastName, record);
            this.ChangeIndex(this.bithdayDictionary, oldRecord.DateOfBirth, record.DateOfBirth, record);
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
        /// Helps to find record by first name.
        /// </summary>
        /// <param name="firstName">FirstName.</param>
        /// <returns>FileCabinetRecord. </returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Helps to find record by last name.
        /// </summary>
        /// <param name="lastName">FirstName.</param>
        /// <returns>FileCabinetRecords. </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Helps to find record by date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>FileCabinetRecord. </returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByBirthDate(DateTime date)
        {
            if (this.bithdayDictionary.ContainsKey(date))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.bithdayDictionary[date]);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Returns all FileCabinetRecords.
        /// </summary>
        /// <returns>Array of FileCabinetRecords.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>((IList<FileCabinetRecord>)this.list);
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

            this.RemoveIndex(this.firstNameDictionary, record.FirstName, record);
            this.RemoveIndex(this.lastNameDictionary, record.LastName, record);
            this.RemoveIndex(this.bithdayDictionary, record.DateOfBirth, record);
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
                    this.AddIndex(this.firstNameDictionary, record.FirstName, record);
                    this.AddIndex(this.lastNameDictionary, record.LastName, record);
                    this.AddIndex(this.bithdayDictionary, record.DateOfBirth, record);
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