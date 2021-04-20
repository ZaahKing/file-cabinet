using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Decorator for FileCabinetService to measure performance.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Measured service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetRecord record)
        {
            return this.Measure(
                "Create method execution duration is {0} ticks.",
                () => this.service.CreateRecord(record));
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord record)
        {
            this.Measure(
                "Edit method execution duration is {0} ticks.",
                () =>
                {
                    this.service.EditRecord(record);
                });
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByBirthDate(DateTime date)
        {
            return this.Measure(
                "FindByBirthDate method execution duration is {0} ticks.",
                () => this.service.FindByBirthDate(date));
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.Measure(
                "FindByFirstName method execution duration is {0} ticks.",
                () => this.service.FindByFirstName(firstName));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.Measure(
                "FindByLastName method execution duration is {0} ticks.",
                () => this.service.FindByLastName(lastName));
        }

        /// <inheritdoc/>
        public FileCabinetRecord FindRecordById(int id)
        {
            return this.Measure(
                "FindRecordById method execution duration is {0} ticks.",
                () => this.service.FindRecordById(id));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.Measure(
                "GetRecords method execution duration is {0} ticks.",
                () => this.service.GetRecords());
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.Measure(
                "GetStat method execution duration is {0} ticks.",
                () => this.service.GetStat());
        }

        /// <inheritdoc/>
        public int GetStatDeleted()
        {
            return this.Measure(
                "GetStatDeleted method execution duration is {0} ticks.",
                () => this.service.GetStatDeleted());
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return this.Measure(
                "GetValidator method execution duration is {0} ticks.",
                () => this.service.GetValidator());
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return this.Measure(
                "MakeSnapshot method execution duration is {0} ticks.",
                () => this.service.MakeSnapshot());
        }

        /// <inheritdoc/>
        public int PurgeStorage()
        {
            return this.Measure(
                "PurgeStorage method execution duration is {0} ticks.",
                () => this.service.PurgeStorage());
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            this.Measure(
                "RemoveRecord method execution duration is {0} ticks.",
                () =>
                {
                    this.service.RemoveRecord(id);
                });
        }

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            return this.Measure(
                "Restore method execution duration is {0} ticks.",
                () => this.service.Restore(snapshot));
        }

        private TOutput Measure<TOutput>(string messageMask, Func<TOutput> measuredAction)
        {
            TOutput output;
            var timer = new Stopwatch();
            timer.Start();
            output = measuredAction();
            timer.Stop();
            Console.WriteLine(messageMask, timer.ElapsedTicks);
            return output;
        }

        private void Measure(string messageMask, Action measuredAction)
        {
            var timer = new Stopwatch();
            timer.Start();
            measuredAction();
            timer.Stop();
            Console.WriteLine(messageMask, timer.ElapsedTicks);
        }
    }
}
