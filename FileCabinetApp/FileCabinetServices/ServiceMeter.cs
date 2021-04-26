using System;
using System.Collections.Generic;
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
            return Measure(
                "Create method execution duration is {0} ticks.",
                () => this.service.CreateRecord(record));
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord record)
        {
            Measure(
                "Edit method execution duration is {0} ticks.",
                () =>
                {
                    this.service.EditRecord(record);
                });
        }

        /// <inheritdoc/>
        public void InsertRecord(FileCabinetRecord record)
        {
            Measure(
                "Insert method execution duration is {0} ticks.",
                () =>
                {
                    this.service.InsertRecord(record);
                });
        }

        /// <inheritdoc/>
        public FileCabinetRecord FindRecordById(int id)
        {
            return Measure(
                "FindRecordById method execution duration is {0} ticks.",
                () => this.service.FindRecordById(id));
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            return Measure(
                "GetRecords method execution duration is {0} ticks.",
                () => this.service.GetRecords());
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return Measure(
                "GetStat method execution duration is {0} ticks.",
                () => this.service.GetStat());
        }

        /// <inheritdoc/>
        public int GetStatDeleted()
        {
            return Measure(
                "GetStatDeleted method execution duration is {0} ticks.",
                () => this.service.GetStatDeleted());
        }

        /// <inheritdoc/>
        public IRecordValidator GetValidator()
        {
            return Measure(
                "GetValidator method execution duration is {0} ticks.",
                () => this.service.GetValidator());
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return Measure(
                "MakeSnapshot method execution duration is {0} ticks.",
                () => this.service.MakeSnapshot());
        }

        /// <inheritdoc/>
        public int PurgeStorage()
        {
            return Measure(
                "PurgeStorage method execution duration is {0} ticks.",
                () => this.service.PurgeStorage());
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            Measure(
                "RemoveRecord method execution duration is {0} ticks.",
                () =>
                {
                    this.service.RemoveRecord(id);
                });
        }

        /// <inheritdoc/>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            return Measure(
                "Restore method execution duration is {0} ticks.",
                () => this.service.Restore(snapshot));
        }

        private static TOutput Measure<TOutput>(string messageMask, Func<TOutput> measuredAction)
        {
            TOutput output;
            var timer = new Stopwatch();
            timer.Start();
            output = measuredAction();
            timer.Stop();
            Console.WriteLine(messageMask, timer.ElapsedTicks);
            return output;
        }

        private static void Measure(string messageMask, Action measuredAction)
        {
            var timer = new Stopwatch();
            timer.Start();
            measuredAction();
            timer.Stop();
            Console.WriteLine(messageMask, timer.ElapsedTicks);
        }
    }
}
