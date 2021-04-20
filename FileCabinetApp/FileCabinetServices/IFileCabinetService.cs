﻿using System;
using System.Collections.Generic;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface of service for filecabinet data.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Add new record.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        /// <returns>Returns Id of new record.</returns>
        int CreateRecord(FileCabinetRecord record);

        /// <summary>
        /// Edit existing record found by Id.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        void EditRecord(FileCabinetRecord record);

        /// <summary>
        /// Helps to find record by date of birth.
        /// </summary>
        /// <param name="date">Date of birth.</param>
        /// <returns>FileCabinetRecord. </returns>
        IEnumerable<FileCabinetRecord> FindByBirthDate(DateTime date);

        /// <summary>
        /// Helps to find record by first name.
        /// </summary>
        /// <param name="firstName">FirstName.</param>
        /// <returns>FileCabinetRecord. </returns>
        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Helps to find record by last name.
        /// </summary>
        /// <param name="lastName">FirstName.</param>
        /// <returns>FileCabinetRecords. </returns>
        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Helps to find record by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>FileCabinetRecord. </returns>
        FileCabinetRecord FindRecordById(int id);

        /// <summary>
        /// Returns all FileCabinetRecords.
        /// </summary>
        /// <returns>Array of FileCabinetRecords.</returns>
        IEnumerable<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Returns count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        int GetStat();

        /// <summary>
        /// Returns count records for purging.
        /// </summary>
        /// <returns>Count of deleted records.</returns>
        int GetStatDeleted();

        /// <summary>
        /// Returns validator used in service.
        /// </summary>
        /// <returns>Validator.</returns>
        IRecordValidator GetValidator();

        /// <summary>
        /// Make file cabinet records snapshot.
        /// </summary>
        /// <returns>FileCabinetServiceSnapshot.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restore file cabinet records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot object.</param>
        /// <returns>Cout of added records.</returns>
        int Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Id for delation.</param>
        void RemoveRecord(int id);

        /// <summary>
        /// Init storage purge proccess.
        /// </summary>
        /// <returns>Count of purged records.</returns>
        int PurgeStorage();
    }
}